using System;
using System.Collections.Generic;
using RecursionTracker.Plugins.PlanetSide2;

namespace RecursionTracker.Plugins.VoicePackCombiner.VoicePack
{
    public class VoicePackMerger
    {
        /// <summary>
        /// Merges the Achievements from otherAchievmentList into the achievements of achievementList
        /// </summary>
        public static void MergeAchievementList(XmlDictionary<string, AchievementOptions> AchievementList, XmlDictionary<string, AchievementOptions> otherAchievementList)
        {
            foreach (var achievementPair in AchievementList)
            {
                var key = achievementPair.Key;
                AchievementOptions achievement = achievementPair.Value;
                AchievementOptions otherAchievement = otherAchievementList[key];
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
            AddNonDefaultSoundsToList(achievement, soundsToAdd);
            AddNonDefaultSoundsToList(otherAchievement, soundsToAdd);

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
                achievement.fileSoundPath = "default";
                achievement.pakSoundPath = null;
                achievement.dynamicSounds.sounds = soundsToAdd.ToArray();
            }
            //else if soundsToAdd.Count == 0, there is nothing to change, achievement has a default sound already (calling LoadNewAchievements() in Merge() makes sure of that)

        }

        /// <summary>
        /// Helper function that gathers all sounds from a achievement, be it old style one sound, or multiple dynamicsounds
        /// and adds them to the "List<> sounds" argument. 
        /// </summary>
        private static void AddNonDefaultSoundsToList(AchievementOptions achievement, List<BasicAchievementSound> sounds)
        {
            //Add old style one sound
            if (achievement.fileSoundPath.ToLower().Trim() != "default")
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
        }

        public static void MergeComponentInformation(ComponentInformation compInfo1, ComponentInformation compInfo2)
        {
            throw new NotImplementedException();
        }
    }
}