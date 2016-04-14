using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursionTracker.Plugins.PlanetSide2;
using RecursionTracker.Plugins.VoicepackCombiner.Voicepack;

namespace RecursionTracker.Plugins.VoicepackCombiner.VoicepackCombinerTest.Voicepack
{
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

            pack.Voicepack.groupManager.achievementList[key] = new AchievementOptions()
            {
                dynamicSounds = new BasicDynamicSoundManager()
                {
                    sounds = new BasicAchievementSound[2]
                }
            };

            pack.Voicepack.groupManager.achievementList[key].dynamicSounds.sounds[0] = new BasicAchievementSound()
            {
                pakSoundFile = findMe0
            };

            pack.Voicepack.groupManager.achievementList[key].dynamicSounds.sounds[1] = new BasicAchievementSound()
            {
                pakSoundFile = findMe1
            };

            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe1, replaceWith1));
            Assert.IsFalse(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe1));
            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, replaceWith1));

            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe0, replaceWith0));
            Assert.IsFalse(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, findMe0));
            Assert.IsTrue(VoicepackCleaner.FindPAKreferenceInVoicepackAndReplace(pack, replaceWith0));
        }

    }
}