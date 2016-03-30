using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursionTracker.Plugins.VoicePackCombiner.Tests;
using RecursionTracker.Plugins.VoicePackCombiner.VoicePack;

namespace RecursionTracker.Plugins.VoicePackCombiner.VoicePackCombinerTest.VoicePack
{
    [TestClass]
    public class EqualComponentInfoTest
    {
        readonly VoicePackExtended _testPack1 = new VoicePackExtended();
        readonly VoicePackExtended _testPack2 = new VoicePackExtended();

        private static ComponentInformation CreateSampleCompInfo()
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

        [TestMethod]
        public void NullVoicePacksHaveEqualComponentInformation()
        {
            Assert.IsTrue(_testPack1.EqualComponentInfo(_testPack2));
        }

        [TestMethod]
        public void DefaultVoicePackHasDifferentCompInfoThanNullVoicePack()
        {
            _testPack1.InitializeToDefault();
            Assert.IsFalse(_testPack1.EqualComponentInfo(_testPack2));
        }

        [TestMethod]
        public void IdenticalSampleVoicePacksHaveEqualComponentInformation()
        {
            _testPack1.InitializeToDefault();
            _testPack1.VoicePack.componentInformation = CreateSampleCompInfo();
            _testPack2.InitializeToDefault();
            _testPack2.VoicePack.componentInformation = CreateSampleCompInfo();

            Assert.IsTrue(_testPack1.EqualComponentInfo(_testPack2));
        }

        [TestMethod]
        public void SlightlyDifferentSampleVoicePacksDontHaveEqualComponentInformation()
        {
            _testPack1.InitializeToDefault();
            _testPack1.VoicePack.componentInformation = CreateSampleCompInfo();
            _testPack2.InitializeToDefault();
            _testPack2.VoicePack.componentInformation = CreateSampleCompInfo();
            _testPack2.VoicePack.componentInformation.author = "Someone Else";
            
            Assert.IsFalse(_testPack1.EqualComponentInfo(_testPack2));
        }

        [TestCategory("Test With File Access"), TestMethod]
        public void TestsWithVoicePackFiles()
        {
            var testPack1 = new VoicePackExtended();
            var testPack2 = new VoicePackExtended();

            testPack1.LoadFromFile(TestData.Pack1);
            testPack2.LoadFromFile(TestData.Pack1);
            Assert.IsTrue(testPack2.EqualComponentInfo(testPack1), "VoicePacks loaded from same file should have same info");

            testPack2.LoadFromFile(TestData.VoicePackPAKBased);
            Assert.IsFalse(testPack2.EqualComponentInfo(testPack1), "VoicePacks loaded from different file should have same info");

            testPack2.LoadFromFile(TestData.Pack1Republished);
            Assert.IsTrue(testPack2.EqualComponentInfo(testPack1), "VoicePacks loaded from republished file should have same info");
        }
    }

    [TestClass]
    public class EqualSoundFileNamesTest
    {
        readonly VoicePackExtended _testPack1 = new VoicePackExtended();
        readonly VoicePackExtended _testPack2 = new VoicePackExtended();

        [TestMethod]
        public void NullVoicePacksHaveEqualSoundFileNames()
        {
            Assert.IsTrue(_testPack1.EqualSoundFilenames(_testPack2));
        }

        [TestMethod]
        public void DefaultVoicePacksHaveEqualSoundFileNames()
        { 
            _testPack1.InitializeToDefault();
            _testPack2.InitializeToDefault();
            Assert.IsTrue(_testPack1.EqualSoundFilenames(_testPack2));
        }

        [TestMethod]
        public void TwoIdenticalSampleVoicePacksHaveEqualSoundFileNames()
        {
            var samplePack1 = VoicePackSampleCreator.Create();
            var samplePack2 = VoicePackSampleCreator.Create();
            
            Assert.IsTrue(samplePack2.EqualSoundFilenames(samplePack1));
        }

        [TestMethod]
        public void SlightlyDifferentSampleVoicePacksDontHaveEqualSoundFileNames()
        {
            var samplePack1 = VoicePackSampleCreator.Create();
            var samplePack2 = VoicePackSampleCreator.Create();
            samplePack2.VoicePack.groupManager.achievementList[VoicePackSampleCreator.SampleAchievements[1]]
                .dynamicSounds.sounds[0].pakSoundFile = "different sound filename";

            Assert.IsFalse(samplePack2.EqualSoundFilenames(samplePack1));

        }

        [TestCategory("Test With File Access"), TestMethod]
        public void TestsWithVoicePacksfromFile()
        {
            var k = new VoicePackExtended();
            var l = new VoicePackExtended();

            k.LoadFromFile(TestData.Pack1);
            l.LoadFromFile(TestData.Pack1);
            Assert.IsTrue(k.EqualSoundFilenames(l), "Voicepack objects loaded from the same file should be equal");
            Assert.IsTrue(l.EqualSoundFilenames(k), "Voicepack objects loaded from the same file should be equal and commutative");

            l.LoadFromFile(TestData.Pack2);
            Assert.IsFalse(l.EqualSoundFilenames(k), "Voicepacks loaded from different files with different content should not be equal");
            Assert.IsFalse(k.EqualSoundFilenames(l), "Voicepacks loaded from different files with different content should not be equal");

            l.LoadFromFile(TestData.Pack1Republished);
            Assert.IsFalse(k.EqualSoundFilenames(l), "This particular republished voicepack differs in one place, because republishing discards the old one sound when there are dynamic sounds (contrived example, tho not intended)");
        }
    }
}