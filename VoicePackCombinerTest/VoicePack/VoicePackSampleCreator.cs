using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecursionTracker.Plugins.PlanetSide2;
using RecursionTracker.Plugins.VoicePackCombiner.VoicePack;

namespace RecursionTracker.Plugins.VoicePackCombiner.VoicePackCombinerTest.VoicePack
{
    /// <summary>
    /// Create a sample VoicePackExtended to test with
    /// </summary>
    public static class VoicePackSampleCreator
    {
        
        public static readonly string[] SampleAchievements = { "PER_KILL", "HUMILIATION", "KILLING_SPREE", "UNSTOPPABLE" };

        public static VoicePackExtended Create()
        {
            var sample = new VoicePackExtended();
            sample.InitializeToDefault();

            //One achievement with 1 'old style' sound
            var achievementList = sample.VoicePack.groupManager.achievementList;
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
    }
}
