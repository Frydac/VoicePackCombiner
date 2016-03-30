using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursionTracker.Plugins.PlanetSide2;
using RecursionTracker.Plugins.VoicePackCombiner;
using RecursionTracker.Plugins.VoicePackCombiner.VoicePackCombinerTest;
using RecursionTracker.Plugins.VoicePackCombiner.VoicePack;

namespace RecursionTracker.Plugins.VoicePackCombiner.VoicePackCombinerTest.VoicePack
{
    /// <summary>
    /// Create a sample VoicePackExtended to test with
    /// </summary>
    static class VoicePackSampleCreator
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
    }


    [TestClass]
    public class VoicePackExtendedTest
    {
        [TestMethod]
        public void InitializeToDefaultTest()
        {
            var testPack = new VoicePackExtended();
            Assert.IsFalse(testPack.IsValidVoicePackLoaded());

            testPack.InitializeToDefault();
            Assert.IsTrue(testPack.IsValidVoicePackLoaded());
        }

        [TestMethod, TestCategory("Test With File Access")]
        public void LoadFromFileReturnValueTest()
        {
            var a = new VoicePackExtended();
            Assert.IsFalse(a.LoadFromFile("not a file"));

            //Cant run this as it generates an exception that is caught before my code can catch it
            //And it shows a popup
            //Assert.IsFalse(a.LoadFromFile(RealFileButNoValidVoicePackFile));

            Assert.IsTrue(a.LoadFromFile(TestData.VoicePackXMLBased));

            Assert.IsTrue(a.LoadFromFile(TestData.VoicePackPAKBased));
        }

        [TestMethod]
        public void IsValidVoicePackLoadedTest()
        {
            //IsValidVoicePackLoaded() is partially implicitly tested by LoadFromFileReturnValueTest
            var a = new VoicePackExtended();
            Assert.IsFalse(a.IsValidVoicePackLoaded());
        }

        //TODO: move or delete
        [TestMethod, TestCategory("Test With File Access")]
        public void MergeTest()
        {
            var testPack1 = new VoicePackExtended();
            var testPack2 = new VoicePackExtended();

            //Load default soundsqw (this code uses global data that doesn't exist while running tests)
            //testPack1.InitializeToDefault();
            //testPack2.InitializeToDefault();

            //testPack1.Merge(testPack2);
            //Assert.IsTrue(testPack1.EqualSoundFilenames(testPack2), "Merging default packs should not change anything");

            testPack1.LoadFromFile(TestData.Pack1);
            testPack2.LoadFromFile(TestData.Pack2);

            var testPack2Ex = new VoicePackExtended();
            testPack2Ex.LoadFromFile(TestData.Pack2andExtraSound);

            testPack1.Merge(testPack2);
            var combinedPack = new VoicePackExtended();
            combinedPack.LoadFromFile(TestData.Pack1and2);
            Assert.IsTrue(testPack1.EqualSoundFilenames(combinedPack));
        } 
    }
}
