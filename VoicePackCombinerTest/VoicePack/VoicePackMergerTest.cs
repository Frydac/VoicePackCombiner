using System;
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
}