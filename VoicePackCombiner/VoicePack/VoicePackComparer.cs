//using System.Diagnostics;
//using System.Linq;
//using RecursionTracker.Plugins.PlanetSide2;

//namespace RecursionTracker.Plugins.VoicePackCombiner
//{
//    public class VoicePackComparer
//    {
//        private VoicePackExtended _achievementOptionsComponentsExtended;

//        public VoicePackComparer(VoicePackExtended achievementOptionsComponentsExtended)
//        {
//            _achievementOptionsComponentsExtended = achievementOptionsComponentsExtended;
//        }

//        /// <summary>
//        /// Now: very specific equals that serves my needs, it only checks the sound filenames
//        /// TODO: possibly not used anymore -> throw out
//        /// </summary>
//        /// <returns>true when equal, false when not equal</returns>
//        public bool EqualSoundFilenames(VoicePackExtended other)
//        {
//            if (!_achievementOptionsComponentsExtended.IsValidVoicePackLoaded() && !other.IsValidVoicePackLoaded())
//                return true;

//            if (!_achievementOptionsComponentsExtended.IsValidVoicePackLoaded() || !other.IsValidVoicePackLoaded())
//                return false;

//            var achievements = _achievementOptionsComponentsExtended.VoicePack.groupManager.achievementList;
//            var otherAchievements = other.VoicePack.groupManager.achievementList;

//            //the number of achievements is presumed to be equal, when its loaded, missing achievements are added

//            foreach (var achievementPair in achievements)
//            {
//                var key = achievementPair.Key;
//                if (!otherAchievements.ContainsKey(key)) return false;
//                var otherAchievement = otherAchievements[key];
//                var achievement = achievementPair.Value;

//                if (!AchievementOptionsOneSoundEqual(achievement, otherAchievement))
//                    return false;

//                if (!AchievementOptionsDynamicSoundsEqual(achievement, otherAchievement))
//                    return false;
//            }

//            Trace.WriteLine("EqualSoundFilenames returned true");
//            return true;
//        }

//        /// <summary>
//        /// Helper function that checks equality of the soundfilenames of the voicepack when it has only 1 sound
//        /// </summary>
//        /// <param name="lhs">left hand side</param>
//        /// <param name="rhs">right hand side</param>
//        /// <returns></returns>
//        public static bool AchievementOptionsOneSoundEqual(AchievementOptions lhs, AchievementOptions rhs)
//        {
//            return lhs.fileSoundPath == rhs.fileSoundPath && lhs.pakSoundPath == rhs.pakSoundPath;
//        }

//        /// <summary>
//        /// Helper function that checks equality of the soundfilenames fo the voicepack when it has 1+ sounds
//        /// </summary>
//        /// <param name="lhs">left hand side</param>
//        /// <param name="rhs">right hand side</param>
//        /// <returns></returns>
//        public static bool AchievementOptionsDynamicSoundsEqual(AchievementOptions lhs, AchievementOptions rhs)
//        {
//            if (rhs.dynamicSounds?.sounds == null && lhs.dynamicSounds?.sounds == null)
//                return true;
//            if (rhs.dynamicSounds?.sounds == null || lhs.dynamicSounds?.sounds == null)
//                return false;

//            if (rhs.dynamicSounds.sounds.Length != lhs.dynamicSounds.sounds.Length)
//                return false;

//            foreach (var sound in rhs.dynamicSounds.sounds)
//            {
//                //try to find other sound according to lambda
//                var otherSound = lhs.dynamicSounds.sounds.SingleOrDefault(
//                    item => item.pakSoundFile == sound.pakSoundFile && item.soundFile == sound.soundFile);
//                if (otherSound == null) return false;
//            }

//            return true;
//        }

//        public bool EqualComponentInfo(VoicePackExtended other)
//        {
//            if (_achievementOptionsComponentsExtended.VoicePack?.componentInformation == null && other.VoicePack?.componentInformation == null)
//                return true;
//            if (_achievementOptionsComponentsExtended.VoicePack?.componentInformation == null || other.VoicePack?.componentInformation == null)
//                return false;

//            var compInfo = _achievementOptionsComponentsExtended.VoicePack.componentInformation;
//            var otherCompInfo = other.VoicePack.componentInformation;
//            return
//                compInfo.name == otherCompInfo.name &&
//                compInfo.author == otherCompInfo.author &&
//                compInfo.description == otherCompInfo.description &&
//                compInfo.sampleImage == otherCompInfo.sampleImage &&
//                compInfo.backupSampleImage == otherCompInfo.backupSampleImage;
//        }
//    }
//}