using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursionTracker.Plugins.VoicepackCombiner.Voicepack
{
    public static class VoicepackCleaner
    {
        /// <summary>
        /// This function prepares voicepacks for merging, they sometimes use the same key to identify a certain resource and
        /// use that key in a dictionary (which needs to be merged and cant contain duplicate keys).
        /// This function will find those keys and change them.
        /// </summary>
        /// <param name="lhs">left hand side, can be null</param>
        /// <param name="rhs">right hand side, can be null</param>
        public static List<string> ResolveComponentDataKeyClashes(VoicepackExtended lhs, VoicepackExtended rhs)
        {
            var clashingKeys = new List<string>();

            if (lhs?.Voicepack?.componentData == null || rhs?.Voicepack?.componentData == null)
                return clashingKeys;

            clashingKeys = FindComponentDataKeyClashes(lhs.Voicepack.componentData, rhs.Voicepack.componentData);

            if (!clashingKeys.Any()) return clashingKeys;

            var lhsNewKeyPrefix = lhs.Voicepack.GetPAKFilePath() ?? lhs.Voicepack.GetFilePath() ?? Guid.NewGuid().ToString();
            ChangeComponentDataKeys(lhs, clashingKeys, lhsNewKeyPrefix);

            var rhsNewKeyPrefix = rhs.Voicepack.GetPAKFilePath() ?? rhs.Voicepack.GetFilePath() ?? Guid.NewGuid().ToString();
            ChangeComponentDataKeys(rhs, clashingKeys, rhsNewKeyPrefix);

            if(lhsNewKeyPrefix == rhsNewKeyPrefix)
                throw new InvalidOperationException($"prefixes should differ, something went wrong, prefix = {lhsNewKeyPrefix}");

            return clashingKeys;
        }

        /// <summary>
        /// Returns a list of all keys present in both dictionaries
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns></returns>
        public static List<string> FindComponentDataKeyClashes(XmlDictionary<string, ComponentData> lhs,
            XmlDictionary<string, ComponentData> rhs)
        {
            var clashingKeys = new List<string>();

            foreach (var lhsDataPair in lhs)
            {
                if (rhs.ContainsKey(lhsDataPair.Key))
                {
                    clashingKeys.Add(lhsDataPair.Key);
                }
            }

            return clashingKeys;
        }

        /// <summary>
        /// Will change all the oldKeys, by prepending the prefix, in the componentData of pack
        /// </summary>
        /// <remarks>presumes all the oldKeys are in the componentData of pack</remarks>
        public static void ChangeComponentDataKeys(VoicepackExtended pack, List<string> oldKeys, string prefix)
        {
            foreach (var oldKey in oldKeys)
            {
                var newKey = $"{prefix}:{oldKey}";
                pack.Voicepack.componentData[newKey] = pack.Voicepack.componentData[oldKey];
                pack.Voicepack.componentData.Remove(oldKey);
                FindPAKreferenceInVoicepackAndReplace(pack, oldKey, newKey);

                Debug.WriteLine($"ComponentData key changed, old key: {oldKey} -- new key: {newKey}");
            }
        }


        /// <summary>
        /// The main program had an error where componentdata from a previous voicepack was still present in a voicepack that does not
        /// use it. This function gets rid of that unused data.
        /// </summary>
        /// <returns>List of removed keys</returns>
        public static List<string> RemoveUnusedComponentData(VoicepackExtended voicepack)
        {
            var keysNotReferenced = new List<string>();

            if (voicepack?.Voicepack?.componentData == null) return keysNotReferenced;

            foreach (var item in voicepack.Voicepack.componentData)
            {
                if (!FindPAKreferenceInVoicepackAndReplace(voicepack, item.Key))
                {
                    keysNotReferenced.Add(item.Key);
                }
            }

            foreach (var keyNotReferenced in keysNotReferenced)
            {
                voicepack.Voicepack.componentData.Remove(keyNotReferenced);
                Debug.WriteLine($"ComponentData removed with key: {keyNotReferenced}");
            }

            return keysNotReferenced;
        }

        /// <summary>
        /// Finds the string that is used to reference componentData to the achievent or background image, and optionally changes it
        /// </summary>
        /// <remarks>
        /// Because of immutable strings, I don't see a way to create the find function separate from
        /// change function. As any reference to a string I try to return and change, would not change the original string.
        /// So can only work with a container of a string.
        /// To avoid code duplication of a search only function: if you only want to search, pass null as newPAKReference
        /// </remarks>
        /// <param name="voicepack">voicepack to search in</param>
        /// <param name="oldPAKReference">string to search for</param>
        /// <param name="newPAKReference">string to replace oldPAKReference with, pass null to search without changing</param>
        /// <returns>true if found (and optionally changed), false if not found</returns>
        public static bool FindPAKreferenceInVoicepackAndReplace(VoicepackExtended voicepack, string oldPAKReference, string newPAKReference=null)
        {
            var groupManager = voicepack.Voicepack.groupManager;

            //check backgroundimage
            if (groupManager.pakBackgroundImage == oldPAKReference)
            {
                if (newPAKReference != null) groupManager.pakBackgroundImage = newPAKReference;
                return true;
            }

            //check achievements/sounds
            foreach (var achievementPair in groupManager.achievementList)
            {
                var achievement = achievementPair.Value;

                //check old style one sound achievement property
                if (achievement.pakSoundPath == oldPAKReference)
                {
                    if (newPAKReference != null) achievement.pakSoundPath = newPAKReference;
                    return true;
                }

                //check new style dynamicsounds achievement property
                if(achievement.dynamicSounds?.sounds == null) continue;
                foreach (var sound in achievement.dynamicSounds.sounds)
                {
                    if (sound.pakSoundFile == oldPAKReference)
                    {
                        if (newPAKReference != null) sound.pakSoundFile = newPAKReference;
                        return true;
                    }
                }
            }

            //not found
            return false;
        }
    }
}
