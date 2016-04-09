using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursionTracker.Plugins.VoicePackCombiner.VoicePackCombinerTest;
using RecursionTracker.Plugins.VoicePackCombiner.VoicePack;

namespace RecursionTracker.Plugins.VoicePackCombiner.VoicePackCombinerTest.VoicePack
{
    /// <summary>
    /// Tests through VoicePackExtended rather than VoicePackComparer as it started there, but then 
    /// got delegated to VoicePackComparer which is also tested through these tests. (not sure what
    /// the best practices are in this case)
    /// </summary>
    [TestClass]
    public class EqualComponentInfoTest
    {
        readonly VoicePackExtended _testPack1 = new VoicePackExtended();
        readonly VoicePackExtended _testPack2 = new VoicePackExtended();

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
            _testPack1.VoicePack.componentInformation = VoicePackSampleCreator.CreateSampleCompInfo();
            _testPack2.InitializeToDefault();
            _testPack2.VoicePack.componentInformation = VoicePackSampleCreator.CreateSampleCompInfo();

            Assert.IsTrue(_testPack1.EqualComponentInfo(_testPack2));
        }

        [TestMethod]
        public void SlightlyDifferentSampleVoicePacksDontHaveEqualComponentInformation()
        {
            _testPack1.InitializeToDefault();
            _testPack1.VoicePack.componentInformation = VoicePackSampleCreator.CreateSampleCompInfo();
            _testPack2.InitializeToDefault();
            _testPack2.VoicePack.componentInformation = VoicePackSampleCreator.CreateSampleCompInfo();
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
            Assert.IsFalse(testPack2.EqualComponentInfo(testPack1), "VoicePacks loaded from different file should not have same info");

            testPack2.LoadFromFile(TestData.Pack1Republished);
            Assert.IsTrue(testPack2.EqualComponentInfo(testPack1), "VoicePacks loaded from republished file should have same info");
        }
    }

    /// <summary>
    /// Tests through VoicePackExtended rather than VoicePackComparer as it started there, but then 
    /// got delegated to VoicePackComparer which is also tested through these tests. In voicepackcomparer this is called
    /// EqualAchievementList 
    /// </summary>
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

    [TestClass]
    public class TestEqualComponentData
    {
        readonly XmlDictionary<string, ComponentData> _compData1 = new XmlDictionary<string, ComponentData>();
        readonly XmlDictionary<string, ComponentData> _compData2 = new XmlDictionary<string, ComponentData>();

        [TestMethod]
        public void NullComponentDataIsEqual()
        {
            Assert.IsTrue(VoicePackComparer.EqualComponentData(null, null));
        }

        [TestMethod]
        public void NullAndEmptyDataDictAreNotEqual()
        {
            Assert.IsFalse(VoicePackComparer.EqualComponentData(null, _compData1));
        }

        [TestMethod]
        public void EmptyComponentDataDictsAreEqual()
        {
            Assert.IsTrue(VoicePackComparer.EqualComponentData(_compData1, _compData2));
        }

        [TestMethod]
        public void DataDictWithDifferentSizeAreNotEqual()
        {
            _compData1["key"] = new ComponentData();

            Assert.IsFalse(VoicePackComparer.EqualComponentData(_compData1, _compData2));
        }

        [TestMethod]
        public void DataDictWithSameSizeButDifferentKeysAreNotEqual()
        {
            _compData1["key"] = new ComponentData();
            _compData2["otherKey"] = new ComponentData();

            Assert.IsFalse(VoicePackComparer.EqualComponentData(_compData1, _compData2));
        }

        [TestMethod]
        public void DataDictsWithSameKeyAndEmptyDataItemAreEqual()
        {
            _compData1["key"] = new ComponentData();
            _compData2["key"] = new ComponentData();

            Assert.IsTrue(VoicePackComparer.EqualComponentData(_compData1, _compData2));
        }

        [TestMethod]
        public void DataDicsWithSameKeyButDifferentDataItemSizesAreNotEqual()
        {
            const int dataItemsSize = 5;
            _compData1["key"] = new ComponentData() {data = new byte[dataItemsSize]};
            _compData2["key"] = new ComponentData() {data = new byte[dataItemsSize + 1]};

            Assert.IsFalse(VoicePackComparer.EqualComponentData(_compData1, _compData2));
        }

        [TestMethod]
        public void DataDicsWithSameKeyAndSameDataItemSizesAreEqual()
        {
            const int dataItemsSize = 5;
            _compData1["key"] = new ComponentData() {data = new byte[dataItemsSize]};
            _compData2["key"] = new ComponentData() {data = new byte[dataItemsSize]};

            Assert.IsTrue(VoicePackComparer.EqualComponentData(_compData1, _compData2));
        }
    }
}