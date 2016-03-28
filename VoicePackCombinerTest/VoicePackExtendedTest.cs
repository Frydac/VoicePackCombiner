using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursionTracker.Plugins.PlanetSide2;
using RecursionTracker.Plugins.VoicePackCombiner;

namespace RecursionTracker.Plugins.VoicePackCombiner.Tests
{
    /// <summary>
    /// TestData on hard drive
    /// </summary>
    static class TestData
    {
        public static readonly string TestDataFolder = @"../../../TestData/";
        public static readonly string Pack1 = Path.Combine(TestDataFolder, "_emile_test_1.rtst_vpk");
        public static readonly string Pack2 = Path.Combine(TestDataFolder, "_emile_test_2.rtst_vpk");
        public static readonly string Pack1Republished = Path.Combine(TestDataFolder, "_emile_test_1_republish.rtst_vpk");
        public static readonly string Pack1and2 = Path.Combine(TestDataFolder, "_emile_test_1_and_2_combined.rtst_vpk");
        public static readonly string Pack2andExtraSound = Path.Combine(TestDataFolder, "_emile_test_2_reinitialized_added_1_sound.rtst_vpk");
        public static readonly string RealFileButNoValidVoicePackFile = Path.Combine(TestDataFolder, "dummyFile.rtst_vpk");
        public static readonly string VoicePackXMLBased = Path.Combine(TestDataFolder, "AL_PACINO.rtst_vpk");
        public static readonly string VoicePackPAKBased = Path.Combine(TestDataFolder, "RAVP.rtst_vpk");


        /// These depend on files from the build folder used by RTLibrary, which should be in the same folder
        /// as VoicePackCombiner project folder, and also depends on RTPluginPS2 being installed in that build
        /// Folder as a Mod.
        public static readonly string buildFolder = @"..\..\..\Build";
        public static readonly string achievementSettings = Path.Combine(buildFolder, @"Settings\AchievementSettings.xml");
        public static readonly string achievementsPAKFile = Path.Combine(buildFolder, @"Mods\3\Components\RecursionAchievements.rtst_pak");

        public static void CheckTestDataExists()
        {
            //just check one file in TestData folder, if the folder is there, probably everything is
            if (!File.Exists(Pack1))
                throw new InvalidDataException("Test Data not found, should be in test folder: " + Path.GetFullPath(TestDataFolder));

            if (!File.Exists(achievementSettings))
                throw new InvalidDataException("AchievementSettings.xml not in the expected path: " + Path.GetFullPath(achievementSettings));
            if (!File.Exists(achievementsPAKFile))
                throw new InvalidDataException("RecursionAchievements.rtst_pak not in the expected path: " + Path.GetFullPath(achievementsPAKFile));
        }
    }

    /// <summary>
    /// Create a sample VoicePack to test with
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
    public class AssemblySetup
    {
        
        [AssemblyInitialize]
        public static void RunOnlyOnceBeforeStartingTests(TestContext context)
        {
            TestData.CheckTestDataExists();
            InitializeProtobuf();
            InitializeGlobalDependencies();
        }
        private static void InitializeProtobuf()
        {
            ProtoBuf.Meta.RuntimeTypeModel.Default.DynamicTypeFormatting += RecursionTracker.Plugins.PlanetSide2.Voicepacks.Default_DynamicTypeFormatting;
            RecursionTracker.Plugins.PlanetSide2.Voicepacks.SetRuntimeTypeModel();
        }

        /// <summary>
        /// Needed to be able to call certain functions that depend on this
        /// </summary>
        private static void InitializeGlobalDependencies()
        {
            PlanetSide2.GlobalVariablesPS2.ACHIEVEMENT_SETTINGS_DEFAULT_PATH = TestData.achievementSettings;
            PlanetSide2.GlobalVariablesPS2.achievements.LoadFromPAKFile(TestData.achievementsPAKFile);
            PlanetSide2.GlobalVariablesPS2.achievementOptions.InitializeOnNull();
            PlanetSide2.GlobalVariablesPS2.achievementOptions.RestoreDefaults();
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

        //Test merge achievement
        // merge achievementlist
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
    }

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
    }
}
