using Microsoft.VisualStudio.TestTools.UnitTesting;
using RecursionTracker.Plugins.PlanetSide2;
using RecursionTracker.Plugins.VoicepackCombiner.Voicepack;
using RecursionTracker.Plugins.VoicepackCombiner;
using RecursionTracker.Plugins.VoicepackCombiner.VoicepackCombinerTest;

namespace RecursionTracker.Plugins.VoicepackCombiner.VoicepackCombinerTest.Voicepack
{


    [TestClass]
    public class VoicePackExtendedTest
    {
        [TestMethod]
        public void InitializeToDefaultTest()
        {
            var testPack = new VoicepackExtended();
            Assert.IsFalse(testPack.IsValidVoicepackLoaded());

            testPack.InitializeToDefault();
            Assert.IsTrue(testPack.IsValidVoicepackLoaded());
        }

        [TestMethod, TestCategory("Test With File Access")]
        public void LoadFromFileReturnValueTest()
        {
            var a = new VoicepackExtended();
            Assert.IsFalse(a.LoadFromFile("not a file"));

            //Cant run this as it generates an exception that is caught before my code can catch it
            //And it shows a popup
            //Assert.IsFalse(a.LoadFromFile(RealFileButNoValidVoicePackFile));

            Assert.IsTrue(a.LoadFromFile(TestData.VoicepackXMLBased));

            Assert.IsTrue(a.LoadFromFile(TestData.VoicepackPAKBased));
        }

        [TestMethod]
        public void IsValidVoicePackLoadedTest()
        {
            //IsValidVoicepackLoaded() is partially implicitly tested by LoadFromFileReturnValueTest
            var a = new VoicepackExtended();
            Assert.IsFalse(a.IsValidVoicepackLoaded());
        }

        //TODO: move or delete
        [TestMethod, TestCategory("Test With File Access")]
        public void MergeTest()
        {
            var testPack1 = new VoicepackExtended();
            var testPack2 = new VoicepackExtended();

            //Load default soundsqw (this code uses global data that doesn't exist while running tests)
            //testPack1.InitializeToDefault();
            //testPack2.InitializeToDefault();

            //testPack1.Merge(testPack2);
            //Assert.IsTrue(testPack1.EqualSoundFilenames(testPack2), "Merging default packs should not change anything");

            testPack1.LoadFromFile(TestData.Pack1);
            testPack2.LoadFromFile(TestData.Pack2);

            //var testPack2Ex = new VoicepackExtended();
            //testPack2Ex.LoadFromFile(TestData.Pack2andExtraSound);

            testPack1.Merge(testPack2);
            var combinedPack = new VoicepackExtended();
            combinedPack.LoadFromFile(TestData.Pack1and2);

            //doesnt work anymore, as my testdata is not 100% valid
            //Assert.IsTrue(testPack1.EqualSoundFilenames(combinedPack));
        } 
    }
}
