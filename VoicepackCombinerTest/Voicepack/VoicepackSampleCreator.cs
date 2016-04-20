using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecursionTracker.Plugins.PlanetSide2;
using RecursionTracker.Plugins.VoicepackCombiner.Voicepack;

namespace RecursionTracker.Plugins.VoicepackCombiner.VoicepackCombinerTest.Voicepack
{
    /// <summary>
    /// Create a sample VoicepackExtended or parts of it to test with
    /// </summary>
    public static class VoicepackSampleCreator
    {
        
        public static readonly string[] SampleAchievements = { "PER_KILL", "HUMILIATION", "KILLING_SPREE", "UNSTOPPABLE" };

        public static VoicepackExtended Create()
        {
            var sample = new VoicepackExtended();
            sample.InitializeToDefault();

            //One achievement with 1 'old style' sound
            var achievementList = sample.Voicepack.groupManager.achievementList;
            achievementList[SampleAchievements[0]].fileSoundPath = "filepath1";
            achievementList[SampleAchievements[0]].pakSoundPath = "pakpath1";

            //One achievement with 2 dynamic sounds
            achievementList[SampleAchievements[1]].dynamicSounds = new BasicDynamicSoundManager();
            var sounds = achievementList[SampleAchievements[1]].dynamicSounds.sounds = new BasicAchievementSound[2];
            sounds[0] = new BasicAchievementSound
            {
                pakSoundFile = SampleAchievements[1] + "_pakSoundFile1",
                soundFile = SampleAchievements[1] + "_soundFile1"
            };
            sounds[1] = new BasicAchievementSound
            {
                pakSoundFile = SampleAchievements[2] + "_pakSoundFile1",
                soundFile = SampleAchievements[2] + "_soundFile1"
            };
            //sounds[0].pakSoundFile = SampleAchievements[1] + "_pakSoundFile1";
            //sounds[0].soundFile = SampleAchievements[1] + "_soundFile1";

            //sounds[1].pakSoundFile = SampleAchievements[2] + "_pakSoundFile2";
            //sounds[1].soundFile = SampleAchievements[2] + "_soundFile2";

            //sounds[1] = CreateBasicAchievementSound(SampleAchievements[2] + "_pakSoundFile2", SampleAchievements[2] + "_soundFile2");

            return sample;
        }
        //private static BasicAchievementSound CreateBasicAchievementSound(string pakSoundFile, string soundFile)
        //{
        //    return new BasicAchievementSound
        //    {
        //        pakSoundFile = pakSoundFile,
        //        soundFile = soundFile
        //    };
        //}
        public static ComponentInformation CreateSampleCompInfo()
        {
            return new ComponentInformation()
            {
                //only relevant fields (I hope ;) )
                author = "author",
                backupSampleImage = "",
                description = "description",
                name = "name",
                sampleImage = "sampleImage"
            };
        }

        public static AchievementOptions CreateOneSoundAchievement(string pakSoundPath, string fileSoundPath)
        {
            var oneSoundAchievement = new AchievementOptions();
            oneSoundAchievement.Initialize();
            oneSoundAchievement.fileSoundPath = fileSoundPath;
            oneSoundAchievement.pakSoundPath = pakSoundPath;
            return oneSoundAchievement;
        }

        /// <summary>
        /// expects pakAndSoundFiles as an array of dimension [x,2] with x the number of sounds to add
        /// it expects this format: {{pakSoundfile, SoundFile}, {pakSoundFile,..}, ..}
        /// </summary>
        /// <param name="pakAndSoundFiles"></param>
        /// <returns></returns>
        public static AchievementOptions CreateDynamicSoundsAchievement(string[,] pakAndSoundFiles)
        {
            var nrOfSounds = pakAndSoundFiles.GetLength(0);

            var dynamicSoundsAchievment = new AchievementOptions();
            dynamicSoundsAchievment.Initialize();
            var sounds = dynamicSoundsAchievment.dynamicSounds.sounds = new BasicAchievementSound[nrOfSounds];

            for (int i = 0; i < nrOfSounds; i++)
            {
                sounds[i] = new BasicAchievementSound
                {
                    pakSoundFile = pakAndSoundFiles[i, 0],
                    soundFile = pakAndSoundFiles[i, 1]
                };
            }

            return dynamicSoundsAchievment;
        }
    }
}
