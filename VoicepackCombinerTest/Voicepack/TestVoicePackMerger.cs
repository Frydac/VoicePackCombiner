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

        public AchievementOptions CreateOneSoundAchievement(string pakSoundPath, string fileSoundPath)
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
        public AchievementOptions CreateDynamicSoundsAchievement(string[,] pakAndSoundFiles)
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

        [TestMethod]
        public void MergeOneSoundAchievementWithDefault()
        {
            var defaultAchievement = new AchievementOptions();
            defaultAchievement.Initialize();

            var oneSoundAchievement = CreateOneSoundAchievement("pakSoundPath", "fileSoundPath");


            VoicepackMerger.MergeAchievement(defaultAchievement, oneSoundAchievement);


            var expectedAchievement = oneSoundAchievement;
            Assert.IsTrue(VoicepackComparer.AchievementOptionsOneSoundEqual(expectedAchievement, defaultAchievement));
        }

        [TestMethod]
        public void MergeOneSoundAchievementsResultInDynamicSoundsAchievement()
        {
            var oneSoundAchievement1 = CreateOneSoundAchievement("pakSoundPath1", "fileSoundPath1");
            var oneSoundAchievement2 = CreateOneSoundAchievement("pakSoundPath2", "fileSoundPath2");


            VoicepackMerger.MergeAchievement(oneSoundAchievement1, oneSoundAchievement2);


            var expectedAchievementSameOrder = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath2", "fileSoundPath2"}});
            Assert.IsTrue(VoicepackComparer.AchievementOptionsDynamicSoundsEqual(
                expectedAchievementSameOrder, oneSoundAchievement1));

            //also check other order
            var expectedAchievementDifferentOrder = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath2", "fileSoundPath2"}, 
                {"pakSoundPath1", "fileSoundPath1"}});
            Assert.IsTrue(VoicepackComparer.AchievementOptionsDynamicSoundsEqual(
                expectedAchievementDifferentOrder, oneSoundAchievement1));
        }

        [TestMethod]
        public void MergeOneSoundAchievementWithDynamicSoundsAchievement()
        {
            var oneSoundAchievement = CreateOneSoundAchievement("pakSoundPath1", "fileSoundPath1");

            var dynamicSoundsAchievment = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath3", "fileSoundPath3"}, 
                {"pakSoundPath2", "fileSoundPath2"}});

            VoicepackMerger.MergeAchievement(oneSoundAchievement, dynamicSoundsAchievment);

            var expectedAchievement  = CreateDynamicSoundsAchievement(new string[3, 2]
            {{"pakSoundPath3", "fileSoundPath3"},
                {"pakSoundPath2", "fileSoundPath2"}, 
                {"pakSoundPath1", "fileSoundPath1"}});

            Assert.IsTrue(VoicepackComparer.AchievementOptionsDynamicSoundsEqual(expectedAchievement, oneSoundAchievement));
        }

        [TestMethod]
        public void MergeDynamicSoundsAchievments()
        {
            var dynamicSoundsAchievment1 = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath3", "fileSoundPath3"}, 
                {"pakSoundPath2", "fileSoundPath2"}});

            var dynamicSoundsAchievment2 = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath4", "fileSoundPath4"}});

            VoicepackMerger.MergeAchievement(dynamicSoundsAchievment1, dynamicSoundsAchievment2);

            var expectedAchievement  = CreateDynamicSoundsAchievement(new string[4, 2]
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
            var dynamicSoundsAchievment1 = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath3", "fileSoundPath3"}, 
                {"pakSoundPath2", "fileSoundPath2"}});

            var dynamicSoundsAchievment2 = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath4", "fileSoundPath4"}});

            var dynamicSoundsAchievment3 = CreateDynamicSoundsAchievement(new string[2, 2]
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

            var key1ExpectedAchievement = CreateDynamicSoundsAchievement(new string[4, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath4", "fileSoundPath4"},
                {"pakSoundPath3", "fileSoundPath3"}, 
                {"pakSoundPath2", "fileSoundPath2"}});

            var key2ExpectedAchievement = CreateDynamicSoundsAchievement(new string[4, 2]
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

        //might have to move
        [TestMethod]
        public void TestFindPAKString()
        {
            VoicepackExtended xmlbased = new VoicepackExtended();
            xmlbased.LoadFromFile(@"c:\Program Files (x86)\Recursion\RecursionTracker\buzzcutpsycho.rtst_vpk");
            VoicepackExtended PAKbased = new VoicepackExtended();
            PAKbased.LoadFromFile(@"c:\Program Files (x86)\Recursion\RecursionTracker\TheOfficeUS_v1.3_YCW.rtst_vpk");

            int a = 10;
            int b = a*a;
            Console.WriteLine(b);
            //pack.Voicepack.groupManager.BackgroundImage << xml based filename
            //pack.Voicepack.groupManager.pakBackgroundImage = "pakBackgroundImage";  << seems to be the only one in use
            //pack.Voicepack.IsFromPAK();
            //pack.Voicepack.groupManager.achievementList[0].pakSoundPath;
            //pack.Voicepack.groupManager.achievementList[0].dynamicSounds.sounds[0].pakSoundFile;
            //pack.Voicepack.groupManager.componentInfo.sampleImage  <- pakBackgroundImage??
            //pack.Voicepack.componentInformation.sampleImage <- same as above? I think this componentinfo is only used for referencing the file it came from
        }
        //null into something

        //sample element into sample element

        [TestMethod, TestCategory("Test With File Access")]
        public void FindPAKReferenceInVoicepackAndChangeIntegrationTest()
        {
            VoicepackExtended pack = new VoicepackExtended();
            pack.LoadFromFile(TestData.VoicepackTheOffice);

            //Just find
            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, "HesDead.ogg_System.Byte", null));

            //Find and replace
            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, "HesDead.ogg_System.Byte", "newstring"));

            //Find replaced string, should not be found
            Assert.IsFalse(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, "HesDead.ogg_System.Byte", null));

            //Find new string, should be found
            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, "newstring", null));

            //manually check the right place of the string
            //should be in HEADSHOT achievement, which is in place 0
            var test = pack.Voicepack.groupManager.achievementList["HEAD_SHOT"].dynamicSounds.sounds[1].pakSoundFile;
            Console.WriteLine(test);
            //		pakSoundFile	"HesDead.ogg_System.Byte"	string HEADSHOT ix 0



            //		pakSoundPath	"RanOver.ogg_System.Byte"	string ROAD_KILL ix 3

        }

    }
}