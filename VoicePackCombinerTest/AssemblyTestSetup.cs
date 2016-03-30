using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RecursionTracker.Plugins.VoicePackCombiner.Tests
{
    /// <summary>
    /// Things to initialize once before any of the tests run
    /// </summary>
    [TestClass]
    public class AssemblyTestSetup
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
}