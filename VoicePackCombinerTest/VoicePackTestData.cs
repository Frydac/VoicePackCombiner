using System.IO;

namespace RecursionTracker.Plugins.VoicePackCombiner.VoicePackCombinerTest
{
    /// <summary>
    /// TestData on hard drive
    /// </summary>
    /// <remarks>paths are relative to .cjproj</remarks>
    static class TestData
    {
        public static readonly string TestDataFolder = @"../../../../TestData/";
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
        public static readonly string buildFolder = @"..\..\..\..\Build";
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
}