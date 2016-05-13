using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursionTracker.Plugins.PlanetSide2;
using RecursionTracker.Plugins.VoicepackCombiner.Voicepack;

namespace RecursionTracker.Plugins.VoicepackCombiner.VoicepackCombinerTest.Voicepack
{
    [TestClass]
    public class TestResolveComponentDataKeyClashes
    {
        VoicepackExtended pack1 = new VoicepackExtended();
        VoicepackExtended pack2 = new VoicepackExtended();

        [TestInitialize]
        public void setup()
        {
            pack1.InitializeToDefault();
            pack2.InitializeToDefault();
        }

        [TestMethod]
        public void resolvePAKBackgroundImageClash()
        {
            var pakBackgroundImageKey = "pakBackgroundImageKey";
            var pack1FileName = "pack1FileName";

            pack1.Voicepack.groupManager.pakBackgroundImage = pakBackgroundImageKey;
            pack1.Voicepack.componentData.Add(pakBackgroundImageKey, new ComponentData());

            pack2.Voicepack.groupManager.pakBackgroundImage = pakBackgroundImageKey;
            pack2.Voicepack.componentData.Add(pakBackgroundImageKey, new ComponentData());

            var result = new List<string>();

            result = VoicepackCleaner.ResolveComponentDataKeyClashes(pack1, pack2);

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(pakBackgroundImageKey, result[0]);
        }

        [TestMethod]
        public void FindComponentKeyClashes()
        {
            // seems so trivial
        }

        [TestMethod]
        public void ChangeComponentDataKeysTest()
        {
            List<string> oldKeys = new List<string>();
            oldKeys.Add("oldKey");

            VoicepackExtended pack = new VoicepackExtended();
            pack.InitializeToDefault();
            pack.Voicepack.componentData.Add("oldKey", new ComponentData());

            VoicepackCleaner.ChangeComponentDataKeys(pack, oldKeys, "prefix");

            Assert.IsTrue(pack.Voicepack.componentData.ContainsKey("prefix:oldKey"));
            Assert.IsFalse(pack.Voicepack.componentData.ContainsKey("oldKey"));
        }


    }

    [TestClass]
    public class TestRemoveUnusedComponentData
    {
        VoicepackExtended pack = new VoicepackExtended();

        [TestInitialize]
        public void setup()
        {
            pack.InitializeToDefault();
        }

        [TestMethod]
        public void RemoveComponentDataItemWithoutReference()
        {
            pack.Voicepack.componentData.Add("key", new ComponentData());
            
            VoicepackCleaner.RemoveUnusedComponentData(pack);

            Assert.IsFalse(pack.Voicepack.componentData.ContainsKey("key"));
        }

        [TestMethod]
        public void DontRemoveComponentDataItemWithReferenceToBackupImage()
        {
            pack.Voicepack.componentData = new XmlDictionary<string, ComponentData>();
            var pakBackgroundImage = "pakBackGroundImage";
            pack.Voicepack.groupManager.pakBackgroundImage = pakBackgroundImage;
            pack.Voicepack.componentData.Add(pakBackgroundImage, new ComponentData());

            VoicepackCleaner.RemoveUnusedComponentData(pack);
            Assert.IsTrue(pack.Voicepack.componentData.ContainsKey(pakBackgroundImage));


        }

        [TestMethod]
        public void DontRemoveComponentDataItemWithReferenceToOldStyleOneSoundAchievment()
        {
            var oneSound = "oneSound";
            pack.Voicepack.groupManager.achievementList["achievementName1"] = new AchievementOptions()
            {
                pakSoundPath = oneSound
            };
            pack.Voicepack.componentData.Add(oneSound, new ComponentData());

            VoicepackCleaner.RemoveUnusedComponentData(pack);

            Assert.IsTrue(pack.Voicepack.componentData.ContainsKey(oneSound));
        }

        [TestMethod]
        public void DontRemoveComponentDataItemWithReferenceToNewStyleDynamicSoundAchievment()
        {
            var dynamicSound = "dynamicSound";
            pack.Voicepack.groupManager.achievementList["achievementName2"] =
                VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2] {{"", ""}, {dynamicSound, ""}});
            pack.Voicepack.componentData.Add(dynamicSound, new ComponentData());

            VoicepackCleaner.RemoveUnusedComponentData(pack);

            Assert.IsTrue(pack.Voicepack.componentData.ContainsKey(dynamicSound));
        }

        ///// <summary>
        ///// This function checks every rtst in a folder and strips them from excess ComponentData
        ///// </summary>
        //[TestMethod]
        //public void fix_test_data()
        //{
        //    var files = Directory.GetFiles(@"c:\Program Files (x86)\Recursion\RecursionTracker\");
        //    //var file = @"c:\Program Files (x86)\Recursion\RecursionTracker\VoicepackCombiner.CurrentCombinedVoicepack.rtst_vpk";
        //    foreach (var file in files)
        //    {
        //        if (!file.EndsWith(".rtst_vpk")) continue;

        //        var voicepack = new VoicepackExtended();
        //        voicepack.LoadFromFile(file);
        //        var removedFiles = voicepack.RemoveUnusedComponentData();
        //        if (removedFiles.Any())
        //        {
        //            var outFile = file.Replace(".rtst_vpk", ".cleaned.rtst_vpk");
        //            voicepack.ExportToFile(outFile);
        //            var outFileRemovedKeys = file.Replace(".rtst_vpk", ".removed_data.txt");
        //            File.WriteAllLines(outFileRemovedKeys, removedFiles);
        //        }
        //    }
        //}

    }
    

    [TestClass]
    public class TestFindPAKReferenceInVoicepackAndChangeIntegration
    {
        VoicepackExtended pack = new VoicepackExtended();
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
            pack.Voicepack.groupManager.pakBackgroundImage = findMe;

            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe));
        }

        [TestMethod]
        public void FindAndReplacePAKBackupImage()
        {
            pack.Voicepack.groupManager.pakBackgroundImage = findMe;

            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe, replaceWith));
            Assert.AreEqual(pack.Voicepack.groupManager.pakBackgroundImage, replaceWith);
        }

        [TestMethod]
        public void FindAndReplaceOldStyleOneSoundPath()
        {
            pack.Voicepack.groupManager.achievementList[key] = new AchievementOptions() {pakSoundPath = findMe};

            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe, replaceWith));
            Assert.AreEqual(pack.Voicepack.groupManager.achievementList[key].pakSoundPath, replaceWith);
        }

        [TestMethod]
        public void FindAndReplaceNewStyleDynamicSounsPath()
        {
            var findMe0 = findMe + "0";
            var findMe1 = findMe + "1";
            var replaceWith1 = replaceWith + "1";
            var replaceWith0 = replaceWith + "0";

            pack.Voicepack.groupManager.achievementList[key] =
                VoicepackSampleCreator.CreateDynamicSoundsAchievement(new string[2, 2] {{findMe0, ""}, {findMe1, ""}});

            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe1, replaceWith1));
            Assert.IsFalse(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe1));
            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, replaceWith1));

            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe0, replaceWith0));
            Assert.IsFalse(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe0));
            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, replaceWith0));
        }

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
            //var test = pack.Voicepack.groupManager.achievementList["HEAD_SHOT"].dynamicSounds.sounds[1].pakSoundFile;
            //Console.WriteLine(test);
            //		pakSoundFile	"HesDead.ogg_System.Byte"	string HEADSHOT ix 0



            //		pakSoundPath	"RanOver.ogg_System.Byte"	string ROAD_KILL ix 3

        }

    }
}