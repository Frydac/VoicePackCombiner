using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursionTracker.Plugins.PlanetSide2;
using RecursionTracker.Plugins.VoicepackCombiner.Voicepack;

namespace RecursionTracker.Plugins.VoicepackCombiner.VoicepackCombinerTest.Voicepack
{
    [TestClass]
    public class VoicepackMergerTest
    {
        readonly VoicepackExtended _testPack1 = new VoicepackExtended();
        readonly VoicepackExtended _testPack2 = new VoicepackExtended();

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TryingToMergeFromANullVoicepackThrows()
        {
            _testPack2.InitializeToDefault();
            _testPack1.Merge(_testPack2);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TryingToMergeInANullVoicepackThrows()
        {
            _testPack1.InitializeToDefault();
            _testPack1.Merge(_testPack2);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TryingToMergeNullAchievementThrows()
        {
            VoicepackMerger.MergeAchievement(new AchievementOptions(), null);
        }

        [TestMethod]
        public void MergeOneSoundAchievementWithDefault()
        {
            var defaultAchievement = new AchievementOptions();
            defaultAchievement.Initialize();

            var oneSoundAchievement = VoicepackSampleCreator.CreateOneSoundAchievement("pakSoundPath", "fileSoundPath");


            VoicepackMerger.MergeAchievement(defaultAchievement, oneSoundAchievement);


            var expectedAchievement = oneSoundAchievement;
            Assert.IsTrue(VoicepackComparer.AchievementOptionsOneSoundEqual(expectedAchievement, defaultAchievement));
        }

        [TestMethod]
        public void MergeOneSoundAchievementsResultInDynamicSoundsAchievement()
        {
            var oneSoundAchievement1 = VoicepackSampleCreator.CreateOneSoundAchievement("pakSoundPath1", "fileSoundPath1");
            var oneSoundAchievement2 = VoicepackSampleCreator.CreateOneSoundAchievement("pakSoundPath2", "fileSoundPath2");


            VoicepackMerger.MergeAchievement(oneSoundAchievement1, oneSoundAchievement2);


            var expectedAchievementSameOrder = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath2", "fileSoundPath2"}});
            Assert.IsTrue(VoicepackComparer.AchievementOptionsDynamicSoundsEqual(
                expectedAchievementSameOrder, oneSoundAchievement1));

            //also check other order
            var expectedAchievementDifferentOrder = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath2", "fileSoundPath2"}, 
                {"pakSoundPath1", "fileSoundPath1"}});
            Assert.IsTrue(VoicepackComparer.AchievementOptionsDynamicSoundsEqual(
                expectedAchievementDifferentOrder, oneSoundAchievement1));
        }

        [TestMethod]
        public void MergeOneSoundAchievementWithDynamicSoundsAchievement()
        {
            var oneSoundAchievement = VoicepackSampleCreator.CreateOneSoundAchievement("pakSoundPath1", "fileSoundPath1");

            var dynamicSoundsAchievment = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath3", "fileSoundPath3"}, 
                {"pakSoundPath2", "fileSoundPath2"}});

            VoicepackMerger.MergeAchievement(oneSoundAchievement, dynamicSoundsAchievment);

            var expectedAchievement  = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[3, 2]
            {{"pakSoundPath3", "fileSoundPath3"},
                {"pakSoundPath2", "fileSoundPath2"}, 
                {"pakSoundPath1", "fileSoundPath1"}});

            Assert.IsTrue(VoicepackComparer.AchievementOptionsDynamicSoundsEqual(expectedAchievement, oneSoundAchievement));
        }

        [TestMethod]
        public void MergeDynamicSoundsAchievments()
        {
            var dynamicSoundsAchievment1 = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath3", "fileSoundPath3"}, 
                {"pakSoundPath2", "fileSoundPath2"}});

            var dynamicSoundsAchievment2 = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath4", "fileSoundPath4"}});

            VoicepackMerger.MergeAchievement(dynamicSoundsAchievment1, dynamicSoundsAchievment2);

            var expectedAchievement  = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[4, 2]
            {{"pakSoundPath2", "fileSoundPath2"}, 
                {"pakSoundPath1", "fileSoundPath1"},
                {"pakSoundPath3", "fileSoundPath3"},
                {"pakSoundPath4", "fileSoundPath4"}});

            Assert.IsTrue(VoicepackComparer.AchievementOptionsDynamicSoundsEqual(expectedAchievement, dynamicSoundsAchievment1));
        }

        //a mix of one and dynamic would have been better, kind of code duplication when making the lists
        [TestMethod]
        public void MergeAchievementsInAchievementList()
        {
            var dynamicSoundsAchievment1 = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath3", "fileSoundPath3"}, 
                {"pakSoundPath2", "fileSoundPath2"}});

            var dynamicSoundsAchievment2 = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath4", "fileSoundPath4"}});

            var dynamicSoundsAchievment3 = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath5", "fileSoundPath5"}, 
                {"pakSoundPath6", "fileSoundPath6"}});

            var achievementList1 = new XmlDictionary<string, AchievementOptions>
            {
                ["key1"] = dynamicSoundsAchievment1,
                ["key2"] = dynamicSoundsAchievment2
            };

            var achievementList2 = new XmlDictionary<string, AchievementOptions>
            {
                ["key1"] = dynamicSoundsAchievment2,
                ["key2"] = dynamicSoundsAchievment3
            };

            VoicepackMerger.MergeAchievementList(achievementList1, achievementList2);

            var key1ExpectedAchievement = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[4, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath4", "fileSoundPath4"},
                {"pakSoundPath3", "fileSoundPath3"}, 
                {"pakSoundPath2", "fileSoundPath2"}});

            var key2ExpectedAchievement = VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[4, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath4", "fileSoundPath4"},
                {"pakSoundPath5", "fileSoundPath5"}, 
                {"pakSoundPath6", "fileSoundPath6"}});

            var expectedList = new XmlDictionary<string, AchievementOptions>();
            expectedList["key1"] = key1ExpectedAchievement;
            expectedList["key2"] = key2ExpectedAchievement;

            Assert.IsTrue(VoicepackComparer.EqualAchievementLists(expectedList, achievementList1));
        }


        [TestMethod]
        public void MergeComponentInformation()
        {
            var compInfo1 = new ComponentInformation()
            {
                //only relevant fields (I hope ;) )
                author = "author1",
                backupSampleImage = "backupSampleImage1",
                description = "description1",
                name = "name1",
                sampleImage = "sampleImage1"
            };

            var compInfo2 = new ComponentInformation()
            {
                //only relevant fields (I hope ;) )
                author = "author2",
                backupSampleImage = "backupSampleImage2",
                description = "description2",
                name = "name2",
                sampleImage = "sampleImage2"
            };

            var expectedCompInfo = new ComponentInformation()
            {
                author = "author1, author2",
                backupSampleImage = "backupSampleImage1",
                description =   "Combined Voicepack: name1, name2",
                name = "Combined Voicepack",
                sampleImage = "sampleImage1"
            };

            //VoicepackMerger.MergeComponentInformation(compInfo1, compInfo2);

            //Assert.IsTrue(VoicepackComparer.EqualComponentInformation(compInfo1, expectedCompInfo));
        }
    }

    [TestClass]
    public class TestMergeComponentData
    { 
        [TestMethod]
        public void MergeSimpleSampleComponentData()
        {
            var compDataDict1 = new XmlDictionary<string, ComponentData>();
            var compDataDict2 = new XmlDictionary<string, ComponentData>();

            var sampleCompData = new ComponentData();
            compDataDict2["key2"] = sampleCompData;

            VoicepackMerger.MergeComponentData(compDataDict1, compDataDict2);

            Assert.IsTrue(compDataDict1.ContainsKey("key2"));
            Assert.AreEqual(compDataDict1["key2"], sampleCompData);
        }


    }
}