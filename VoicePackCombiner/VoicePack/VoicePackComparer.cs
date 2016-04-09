

using System.Linq;
using System.Runtime.Remoting.Messaging;
using RecursionTracker.Plugins.PlanetSide2;

namespace RecursionTracker.Plugins.VoicePackCombiner
{
    public class VoicePackComparer
    {
        /// <summary>
        /// Compare ComponentInformation objects for equality.
        /// </summary>
        /// <param name="lhs">left hand side, assumed not to be null</param>
        /// <param name="rhs">right hand side, assumed not to be null</param>
        /// <returns>true if equal, false if not equal</returns>
        public static bool EqualComponentInformation(ComponentInformation lhs, ComponentInformation rhs)
        {
            return
                lhs.name == rhs.name &&
                lhs.author == rhs.author &&
                lhs.description == rhs.description &&
                lhs.sampleImage == rhs.sampleImage &&
                lhs.backupSampleImage == rhs.backupSampleImage;
        }

        /// <summary>
        /// Compare ComponentData objects for equality
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns>true if equal, false if not equal</returns>
        public static bool EqualComponentData(XmlDictionary<string, ComponentData> lhs,
            XmlDictionary<string, ComponentData> rhs)
        {
            if (lhs == null && rhs == null)
                return true;
            if (lhs == null || rhs == null)
                return false;

            if (ComponentDataCollectionSize(lhs) != ComponentDataCollectionSize(rhs))
                return false;

            foreach (var compDataPair in lhs)
            {
                var key = compDataPair.Key;
                var compData = compDataPair.Value;
                if (!rhs.ContainsKey(key)) return false;
                var otherCompData = rhs[key];

                if (!EqualComponentDataItem(compData, otherCompData))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compares one ComponentData object with another, does not check the actual data:
        /// if filename, datatype and data length are equal, they are presumed equal.
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns>true if equal, false if not equal</returns>
        private static bool EqualComponentDataItem(ComponentData lhs, ComponentData rhs)
        {
            if (lhs.fileName != rhs.fileName) return false;
            if (lhs.dataType != rhs.dataType) return false;

            if (lhs.data == null && rhs.data == null)
                return true;
            if (lhs.data == null || rhs.data == null)
                return false;
            if(lhs.data != null && rhs.data != null)
                if (lhs.data.Length != rhs.data.Length)
                    return false;

            return true;
        }

        /// <summary>
        /// Helper function that calculates the Size of a specific XmlDictionary, as that type doesnt have a lenght property
        /// </summary>
        /// <param name="componentData"></param>
        /// <returns></returns>
        private static int ComponentDataCollectionSize(XmlDictionary<string, ComponentData> componentData)
        {
            int counter = 0;
            foreach (var component in componentData)
            {
                counter++;
            }
            return counter;
        }

        /// <summary>
        /// Compare AchievementLists
        /// Presumes equal amount of achievements in each list
        /// </summary>
        /// <param name="lhs">left hand side, assumed not to be null</param>
        /// <param name="rhs">right hand side, assumed not to be null</param>
        /// <returns>true if equal, false if not equal</returns>
        public static bool EqualAchievementLists(XmlDictionary<string, AchievementOptions> lhs, XmlDictionary<string, AchievementOptions> rhs)
        {
            //the number of achievements is presumed to be equal: when its loaded, missing achievements are added
            //so we can loop through one list, find them in the second, and be sure the other has no extra items
            //when all equivalents are found.

            foreach (var achievementPair in lhs)
            {
                var key = achievementPair.Key;
                if (!rhs.ContainsKey(key)) return false;
                var otherAchievement = rhs[key];
                var achievement = achievementPair.Value;

                if (!VoicePackComparer.AchievementOptionsOneSoundEqual(achievement, otherAchievement))
                    return false;

                if (!VoicePackComparer.AchievementOptionsDynamicSoundsEqual(achievement, otherAchievement))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Helper function that checks equality of the soundfilenames of the voicepack when it has only 1 sound
        /// </summary>
        /// <param name="lhs">left hand side, assumed not to be null</param>
        /// <param name="rhs">right hand side, assumed not to be null</param>
        public static bool AchievementOptionsOneSoundEqual(AchievementOptions lhs, AchievementOptions rhs)
        {
            return lhs.fileSoundPath == rhs.fileSoundPath && lhs.pakSoundPath == rhs.pakSoundPath;
        }

        /// <summary>
        /// Helper function that checks equality of the soundfilenames fo the voicepack when it has 1+ sounds
        /// </summary>
        /// <param name="lhs">left hand side</param>
        /// <param name="rhs">right hand side</param>
        /// <returns></returns>
        public static bool AchievementOptionsDynamicSoundsEqual(AchievementOptions lhs, AchievementOptions rhs)
        {
            if (rhs?.dynamicSounds?.sounds == null && lhs?.dynamicSounds?.sounds == null)
                return true;
            if (rhs?.dynamicSounds?.sounds == null || lhs?.dynamicSounds?.sounds == null)
                return false;

            if (rhs.dynamicSounds.sounds.Length != lhs.dynamicSounds.sounds.Length)
                return false;

            foreach (var sound in rhs.dynamicSounds.sounds)
            {
                //try to find other sound according to lambda
                var otherSound = lhs.dynamicSounds.sounds.SingleOrDefault(
                    item => item.pakSoundFile == sound.pakSoundFile && item.soundFile == sound.soundFile);
                if (otherSound == null) return false;
            }

            return true;
        }
    }
}