using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursionTracker.Plugins.PlanetSide2;
using RecursionTracker.Plugins.VoicePackCombiner.VoicePack;

namespace RecursionTracker.Plugins.VoicePackCombiner.VoicePackCombinerTest.VoicePack
{
    [TestClass]
    public class VoicePackMergerTest
    {
        readonly VoicePackExtended _testPack1 = new VoicePackExtended();
        readonly VoicePackExtended _testPack2 = new VoicePackExtended();

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TryingToMergeFromANullVoicePackThrows()
        {
            _testPack2.InitializeToDefault();
            _testPack1.Merge(_testPack2);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TryingToMergeInANullVoicePackThrows()
        {
            _testPack1.InitializeToDefault();
            _testPack1.Merge(_testPack2);
        }

        //- Test merge achievement
        //- merge achievementlist
        //Test merge componentInfo
        //Test merge componentdata

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void TryingToMergeNullAchievementThrows()
        {
            VoicePackMerger.MergeAchievement(new AchievementOptions(), null);
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


            VoicePackMerger.MergeAchievement(defaultAchievement, oneSoundAchievement);


            var expectedAchievement = oneSoundAchievement;
            Assert.IsTrue(VoicePackComparer.AchievementOptionsOneSoundEqual(expectedAchievement, defaultAchievement));
        }

        [TestMethod]
        public void MergeOneSoundAchievementsResultInDynamicSoundsAchievement()
        {
            var oneSoundAchievement1 = CreateOneSoundAchievement("pakSoundPath1", "fileSoundPath1");
            var oneSoundAchievement2 = CreateOneSoundAchievement("pakSoundPath2", "fileSoundPath2");


            VoicePackMerger.MergeAchievement(oneSoundAchievement1, oneSoundAchievement2);


            var expectedAchievementSameOrder = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath1", "fileSoundPath1"}, 
                {"pakSoundPath2", "fileSoundPath2"}});
            Assert.IsTrue(VoicePackComparer.AchievementOptionsDynamicSoundsEqual(
                expectedAchievementSameOrder, oneSoundAchievement1));

            //also check other order
            var expectedAchievementDifferentOrder = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath2", "fileSoundPath2"}, 
                {"pakSoundPath1", "fileSoundPath1"}});
            Assert.IsTrue(VoicePackComparer.AchievementOptionsDynamicSoundsEqual(
                expectedAchievementDifferentOrder, oneSoundAchievement1));
        }

        [TestMethod]
        public void MergeOneSoundAchievementWithDynamicSoundsAchievement()
        {
            var oneSoundAchievement = CreateOneSoundAchievement("pakSoundPath1", "fileSoundPath1");

            var dynamicSoundsAchievment = CreateDynamicSoundsAchievement(new string[2, 2]
            {{"pakSoundPath3", "fileSoundPath3"}, 
                {"pakSoundPath2", "fileSoundPath2"}});

            VoicePackMerger.MergeAchievement(oneSoundAchievement, dynamicSoundsAchievment);

            var expectedAchievement  = CreateDynamicSoundsAchievement(new string[3, 2]
            {{"pakSoundPath3", "fileSoundPath3"},
                {"pakSoundPath2", "fileSoundPath2"}, 
                {"pakSoundPath1", "fileSoundPath1"}});

            Assert.IsTrue(VoicePackComparer.AchievementOptionsDynamicSoundsEqual(expectedAchievement, oneSoundAchievement));
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

            VoicePackMerger.MergeAchievement(dynamicSoundsAchievment1, dynamicSoundsAchievment2);

            var expectedAchievement  = CreateDynamicSoundsAchievement(new string[4, 2]
            {{"pakSoundPath2", "fileSoundPath2"}, 
                {"pakSoundPath1", "fileSoundPath1"},
                {"pakSoundPath3", "fileSoundPath3"},
                {"pakSoundPath4", "fileSoundPath4"}});

            Assert.IsTrue(VoicePackComparer.AchievementOptionsDynamicSoundsEqual(expectedAchievement, dynamicSoundsAchievment1));
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

            VoicePackMerger.MergeAchievementList(achievementList1, achievementList2);

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

            Assert.IsTrue(VoicePackComparer.EqualAchievementLists(expectedList, achievementList1));
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
                description =   "Combined VoicePack: name1, name2",
                name = "Combined VoicePack",
                sampleImage = "sampleImage1"
            };

            //VoicePackMerger.MergeComponentInformation(compInfo1, compInfo2);

            //Assert.IsTrue(VoicePackComparer.EqualComponentInformation(compInfo1, expectedCompInfo));
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

            VoicePackMerger.MergeComponentData(compDataDict1, compDataDict2);

            Assert.IsTrue(compDataDict1.ContainsKey("key2"));
            Assert.AreEqual(compDataDict1["key2"], sampleCompData);
        }

        //might have to move
        [TestMethod]
        public void TestFindPAKString()
        {
            VoicePackExtended xmlbased = new VoicePackExtended();
            xmlbased.LoadFromFile(@"c:\Program Files (x86)\Recursion\RecursionTracker\buzzcutpsycho.rtst_vpk");
            VoicePackExtended PAKbased = new VoicePackExtended();
            PAKbased.LoadFromFile(@"c:\Program Files (x86)\Recursion\RecursionTracker\TheOfficeUS_v1.3_YCW.rtst_vpk");

            int a = 10;
            int b = a*a;
            Console.WriteLine(b);
            //pack.VoicePack.groupManager.BackgroundImage << xml based filename
            //pack.VoicePack.groupManager.pakBackgroundImage = "pakBackgroundImage";  << seems to be the only one in use
            //pack.VoicePack.IsFromPAK();
            //pack.VoicePack.groupManager.achievementList[0].pakSoundPath;
            //pack.VoicePack.groupManager.achievementList[0].dynamicSounds.sounds[0].pakSoundFile;
            //pack.VoicePack.groupManager.componentInfo.sampleImage  <- pakBackgroundImage??
            //pack.VoicePack.componentInformation.sampleImage <- same as above? I think this componentinfo is only used for referencing the file it came from
        }
        //null into something

        //sample element into sample element

        [TestMethod, TestCategory("Test With File Access")]
        public void FindPAKReferenceInVoicePackAndChangeIntegrationTest()
        {
            VoicePackExtended pack = new VoicePackExtended();
            pack.LoadFromFile(TestData.VoicePackTheOffice);

            //Just find
            Assert.IsTrue(VoicePackMerger.FindPAKreferenceInVoicePackAndReplace(pack, "HesDead.ogg_System.Byte", null));

            //Find and replace
            Assert.IsTrue(VoicePackMerger.FindPAKreferenceInVoicePackAndReplace(pack, "HesDead.ogg_System.Byte", "newstring"));

            //Find replaced string, should not be found
            Assert.IsFalse(VoicePackMerger.FindPAKreferenceInVoicePackAndReplace(pack, "HesDead.ogg_System.Byte", null));

            //Find new string, should be found
            Assert.IsTrue(VoicePackMerger.FindPAKreferenceInVoicePackAndReplace(pack, "newstring", null));

            //manually check the right place of the string
            //should be in HEADSHOT achievement, which is in place 0
            var test = pack.VoicePack.groupManager.achievementList["HEAD_SHOT"].dynamicSounds.sounds[1].pakSoundFile;
            Console.WriteLine(test);
            //		pakSoundFile	"HesDead.ogg_System.Byte"	string HEADSHOT ix 0



            //		pakSoundPath	"RanOver.ogg_System.Byte"	string ROAD_KILL ix 3

        }

    }

    [TestClass]
    public class TestFindPAKReferenceInVoicePackAndChangeIntegration
    {
        VoicePackExtended pack = new VoicePackExtended();
        private const string findMe = "stringToFind";
        private const string replaceWith = "stringToReplaceWith";
        private const string key = "key";

        [TestInitialize]
        public void setup()
        {
            pack.InitializeToDefault();
        }

        [TestMethod]
        public void FindPAKBackupImage()
        {
            pack.VoicePack.groupManager.pakBackgroundImage = findMe;

            Assert.IsTrue(VoicePackMerger.FindPAKreferenceInVoicePackAndReplace(pack, findMe));
        }

        [TestMethod]
        public void FindAndReplacePAKBackupImage()
        {
            pack.VoicePack.groupManager.pakBackgroundImage = findMe;

            Assert.IsTrue(VoicePackMerger.FindPAKreferenceInVoicePackAndReplace(pack, findMe, replaceWith));
            Assert.AreEqual(pack.VoicePack.groupManager.pakBackgroundImage, replaceWith);
        }

        [TestMethod]
        public void FindAndReplaceOldStyleOneSoundPath()
        {
            pack.VoicePack.groupManager.achievementList[key] = new AchievementOptions() {pakSoundPath = findMe};

            Assert.IsTrue(VoicePackMerger.FindPAKreferenceInVoicePackAndReplace(pack, findMe, replaceWith));
            Assert.AreEqual(pack.VoicePack.groupManager.achievementList[key].pakSoundPath, replaceWith);
        }

        [TestMethod]
        public void FindAndReplaceNewStyleDynamicSounsPath()
        {
            //pack.VoicePack.groupManager.achievementList[key] = new AchievementOptions()
            //        { sounds = new BasicAchievementSound[2] } };
        }
    }
}