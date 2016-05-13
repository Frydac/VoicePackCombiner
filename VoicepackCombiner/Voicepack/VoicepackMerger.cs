using System;
using System.Collections.Generic;
using System.Diagnostics;
using RecursionTracker.Plugins.PlanetSide2;

namespace RecursionTracker.Plugins.VoicepackCombiner.Voicepack
{
    public class VoicepackMerger
    {
        private const string _defaultfileSoundPath = "default";

        public static void Merge(VoicepackExtended voicepack, VoicepackExtended otherVoicepack)
        {
            VoicepackCleaner.RemoveUnusedComponentData(voicepack);
            VoicepackCleaner.RemoveUnusedComponentData(otherVoicepack);
            VoicepackCleaner.ResolveComponentDataKeyClashes(voicepack, otherVoicepack);

            MergeAchievementList(voicepack.Voicepack.groupManager.achievementList, otherVoicepack.Voicepack.groupManager.achievementList);
            MergeComponentData(voicepack.Voicepack.componentData, otherVoicepack.Voicepack.componentData);
            //TODO
            //VoicepackMerger.MergeComponentInformation(Voicepack.componentInformation, other.Voicepack.componentInformation);
            
        }
        /// <summary>
        /// Merges the Achievements from otherAchievmentList into the achievements of achievementList
        /// </summary>
        public static void MergeAchievementList(XmlDictionary<string, AchievementOptions> AchievementList, XmlDictionary<string, AchievementOptions> otherAchievementList)
        {
            foreach (var achievementPair in AchievementList)
            {
                var achievement = achievementPair.Value;
                var otherAchievement = otherAchievementList[achievementPair.Key];

                MergeAchievement(achievement, otherAchievement);
            }
        }

        /// <summary>
        /// Merges otherAchievement into achievement
        /// </summary>
        public static void MergeAchievement(AchievementOptions achievement, AchievementOptions otherAchievement)
        {
            if (achievement == null || otherAchievement == null)
                throw new ArgumentNullException();

            //Gather all non-default sounds from both achievements
            var soundsToAdd = new List<BasicAchievementSound>();
            soundsToAdd.AddRange(GetAllNonDefaultSounds(achievement));
            soundsToAdd.AddRange(GetAllNonDefaultSounds(otherAchievement));


            if (soundsToAdd.Count == 1)
            {
                //Create old style one sound achievement
                achievement.fileSoundPath = soundsToAdd[0].soundFile;
                achievement.pakSoundPath = soundsToAdd[0].pakSoundFile;
                achievement.dynamicSounds = null;
            }
            else if (soundsToAdd.Count > 1)
            {
                //Create dynamicSounds multi sound achievement
                achievement.fileSoundPath = _defaultfileSoundPath;
                achievement.pakSoundPath = null;
                if (achievement.dynamicSounds == null)
                    achievement.dynamicSounds = new BasicDynamicSoundManager();
                achievement.dynamicSounds.sounds = soundsToAdd.ToArray();
            }
            //else if soundsToAdd.Count == 0, there is nothing to change, achievement has a default sound already (it is added during loading)

        }

        /// <summary>
        /// Helper function that gathers all sounds from a achievement, be it old style one sound, or multiple dynamicsounds
        /// and returns them in one IEnumerable. 
        /// </summary>
        private static IEnumerable<BasicAchievementSound> GetAllNonDefaultSounds(AchievementOptions achievement)
        {
            var sounds = new List<BasicAchievementSound>();

            //Add old style one sound
            if (achievement.fileSoundPath.ToLower().Trim() != _defaultfileSoundPath)
            {
                var soundToAdd = new BasicAchievementSound
                {
                    pakSoundFile = achievement.pakSoundPath,
                    soundFile = achievement.fileSoundPath
                };
                sounds.Add(soundToAdd);
            }

            //Add dynamic sounds
            if (achievement.dynamicSounds?.sounds != null)
            {
                sounds.AddRange(achievement.dynamicSounds.sounds);
            }

            return sounds;
        }
        
        public static void MergeComponentInformation(ComponentInformation compInfo1, ComponentInformation compInfo2)
        {
            //TODO
            //only author, description and name
            throw new NotImplementedException();
        }

        /// <summary>
        /// Merges otherComponentData into componentData
        /// </summary>
        /// <param name="componentData">presumed not to be null (Initialize on null is performed when loading voicepacks)</param>
        /// <param name="otherComponentData">presumed not to be null (Initialize on null is performed when loading voicepacks)</param>
        public static void MergeComponentData(XmlDictionary<string, ComponentData> componentData,
            XmlDictionary<string, ComponentData> otherComponentData)
        {
            foreach (var otherData in otherComponentData)
            {
                if (componentData.ContainsKey(otherData.Key))
                {
                    //throw new InvalidOperationException("Trying to merge with voicepack with identical key for some sound/resource.\n Key: " + otherData.Key);
                    //TODO: possible workaround: create new key and update all references (e.g. AchievementOptions.pakSoundPath, 
                    Debug.WriteLine("Duplicate key detected while merging: " + otherData.Key + Environment.NewLine);
                }
                componentData[otherData.Key] = otherData.Value;
            }

        }
    }
}