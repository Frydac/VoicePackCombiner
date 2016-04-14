using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecursionTracker.Plugins.VoicepackCombiner.Voicepack
{
    public class VoicepackCleaner
    {
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
