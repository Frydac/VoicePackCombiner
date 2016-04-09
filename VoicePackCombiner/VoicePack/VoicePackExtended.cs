using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using RecursionTracker.Plugins.PlanetSide2;

namespace RecursionTracker.Plugins.VoicePackCombiner.VoicePack
{
    /// <summary>
    /// VoicePackExtended represents an extension to AchievementOptionsComponents (which is ao. the in memory representation of VoicePacks)
    /// When loading/saving voicepack files from/to a file using AchievementOptionsComponents,
    /// some global variables get written so that the loaded file is always used as the current voicepack.
    /// This is a wrapper class to decouple the loading of the voicepack from a file, 
    /// from the actual using of the voicepack by the main program.
    /// 
    /// VoicePackExtended also functions as a facade to VoicePackMerger and VoicePackComparer.
    /// </summary>
    /// 
    /// <remarks>
    /// I use the name "VoicePack" in stead of the "AchievmentOptionsComponents". 
    /// "AchievmentOptions" is a separate class, represents one achievement, and is used in the AchievmentList (dictionary/map).
    /// However the name achievementOptions is also found in GlobalVariablesPS2.achievementOptions for example, but there it is
    /// actually a AchievmentOptionsComponents instance. 
    /// </remarks>
    public class VoicePackExtended
    {
        /// Global Variables to take care/keep track of:
        /// GlobalVariablesPS2.achievementOptions: the voicepack instance that gets used by the main program, gets changed during loading/saving
        /// GlobalVariablesPS2.loadedVoicePack: gets set when loading from a PAK based voicepack file
        /// GlobalVariablesPS2.loadedVoicePackConfigFile: points to the xml based voicepack file
        /// GlobalVariablesPS2.usingPAK: boolean that indicates if the voicepack is loaded from a pak, or xml based voicepack file

        public AchievementOptionsComponents VoicePack { get; set; } = null;
        public string LoadedVoicePack { get; set; }
        public string LoadedVoicePackConfigFile { get; set; }
        public bool UsingPAK { get; set; }

        /// <summary>
        /// Creates a default soundpack from null
        /// </summary>
        public void InitializeToDefault()
        {
            VoicePack = new AchievementOptionsComponents();
            VoicePack.InitializeOnNull(); 
            VoicePack.RestoreDefaults(); 
        }

        /// <summary>
        /// Creates a reference to the global AchievementOptions and all of the global state affected by loading/saving
        /// the achievementOptions(Components)
        /// </summary>
        public void GetFromGlobal()
        {
            VoicePack = GlobalVariablesPS2.achievementOptions;
            LoadedVoicePack = GlobalVariablesPS2.loadedVoicePack;
            LoadedVoicePackConfigFile = GlobalVariablesPS2.loadedVoicePackConfigFile;
            UsingPAK = GlobalVariablesPS2.usingPAK;
        }

        /// <summary>
        /// Sets this voicepack as the GlobalVariablesPS2.achievementOptions, which causes this.VoicePack to be used by the main program.
        /// </summary>
        public void SetAsGlobal()
        {
            GlobalVariablesPS2.achievementOptions = VoicePack;
            GlobalVariablesPS2.loadedVoicePack = LoadedVoicePack;
            GlobalVariablesPS2.loadedVoicePackConfigFile = LoadedVoicePackConfigFile;
            GlobalVariablesPS2.usingPAK = UsingPAK;
        }

        /// <summary>
        /// Checks if this voicepack is loaded as global voicepack, aka is in use by the main program
        /// </summary>
        /// <remarks> 
        /// When the user loads a new voicepack via the main program, one of the loadedpak strings should be different,
        /// while the achievementOptions reference stays the same.
        /// Its ok if the voicepack content is changed in the main program, its still the same loaded voicepack.
        /// </remarks>
        public bool IsGlobal()
        {
            return GlobalVariablesPS2.loadedVoicePack == this.LoadedVoicePack
                   && GlobalVariablesPS2.loadedVoicePackConfigFile == this.LoadedVoicePackConfigFile;
        }


        /// <summary>
        /// Loads the voicepack from file into this instance. Restores any global state that is altered during this operation.
        /// </summary>
        /// <remarks> 
        /// the function achievementOptions.LoadFromPAKFile(string) will always load itself into the global instance, thats why we need
        /// the workaround in this function to load it in (move it in) a seperate instance.
        /// </remarks>
        public bool LoadFromFile(string filename)
        {
            var globalBackup = new VoicePackExtended();

            globalBackup.GetFromGlobal();
            {
                GlobalVariablesPS2.achievementOptions = new AchievementOptionsComponents();
                GlobalVariablesPS2.achievementOptions.LoadFromPAKFile(filename);
                this.GetFromGlobal();
            }
            globalBackup.SetAsGlobal();

            return IsValidVoicePackLoaded();
        }

        /// <summary>
        /// Returns true if this contains a valid voicepack, i.e. a voicepack that contains something rather than nothing
        /// </summary>
        /// <returns></returns>
        public bool IsValidVoicePackLoaded()
        {
            //This is not a complete validation, but trail and error has shown this to be enough
            return VoicePack?.groupManager?.achievementList != null 
                && VoicePack.groupManager.achievementList.Count != 0;
        }

        /// <summary>
        /// Saves this.VoicePack to a binary voicepack file. Makes sure any global state that might be altered during this
        /// operation is restored.
        /// </summary>
        /// <remarks> see remarks LoadFromFile() </remarks>
        public void ExportToFile(string filename)
        {
            if (!IsValidVoicePackLoaded()) return;

            var globalBackup = new VoicePackExtended();
            globalBackup.GetFromGlobal();
            {
                this.SetAsGlobal();
                this.VoicePack.CreatePAKFile(filename);
            }
            globalBackup.SetAsGlobal();
        }

#region merge functions
        /// <summary>
        /// Merges the voicepack with filename into this (this can be empty)
        /// Returns true if it was able to load the voicepack from disk
        /// </summary>
        public bool Merge(string voicePackFilename)
        {
            VoicePackExtended other = new VoicePackExtended();
            if (other.LoadFromFile(voicePackFilename))
            {
                this.Merge(other);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Merges the achievementsOptionsComponens (aka voicepack) of other into this
        /// </summary>
        /// <param name="other"></param>
        public void Merge(VoicePackExtended other)
        {
            if(!IsValidVoicePackLoaded())
                throw new InvalidOperationException();
            if(other == null || !other.IsValidVoicePackLoaded())
                throw new ArgumentNullException();

            VoicePackMerger.Merge(this, other);
        }
#endregion

#region equals functions
        /// <summary>
        /// Checks if this.voicepack refers to the same sounds as other.voicepack
        /// </summary>
        /// <returns>true when equal, false when not equal</returns>
        public bool EqualSoundFilenames(VoicePackExtended other)
        {
            if (!this.IsValidVoicePackLoaded() && !other.IsValidVoicePackLoaded())
                return true;

            if (!this.IsValidVoicePackLoaded() || !other.IsValidVoicePackLoaded())
                return false;

            var achievements = VoicePack.groupManager.achievementList;
            var otherAchievements = other.VoicePack.groupManager.achievementList;

            return VoicePackComparer.EqualAchievementLists(achievements, otherAchievements);
        }

        public bool EqualComponentInfo(VoicePackExtended other)
        {
            if (VoicePack?.componentInformation == null && other.VoicePack?.componentInformation == null)
                return true;
            if (VoicePack?.componentInformation == null || other.VoicePack?.componentInformation == null)
                return false;

            return VoicePackComparer.EqualComponentInformation(VoicePack.componentInformation,
                other.VoicePack.componentInformation);
        }
#endregion

        /// <summary>
        /// Produces a formatted string representing the contents of this instance mainly used for debugging purposes.
        /// TODO: refactor into its own class, and remove some code duplication
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            if (VoicePack == null) return "VoicePack == null"; 

            var output = new StringBuilder();

            output.AppendLine();
            output.AppendFormat("{0,-30}{1,-30}{2}", "LoadedVoicePack: ", LoadedVoicePack, Environment.NewLine);
            output.AppendFormat("{0,-30}{1,-30}{2}", "LoadedVoicePackConfigFile: ", LoadedVoicePackConfigFile, Environment.NewLine);
            output.AppendFormat("{0,-30}{1,-30}{2}", "UsingPAK: ", UsingPAK, Environment.NewLine);

            var groupManager = VoicePack.groupManager;
            if (groupManager == null)
            {
                output.AppendLine("groupManager is null, nothing further to print");
                return output.ToString();
            }

            var componentInfo = groupManager.componentInfo;
            if (VoicePack.groupManager.componentInfo == null)
            {
                output.AppendLine("componentInfo is null");
            }
            else
            {
                output.AppendLine("ComponentInfo:");
                output.AppendFormat("{0,-30}{1,-30}{2}", " Name: ", componentInfo.name, Environment.NewLine);
                output.AppendFormat("{0,-30}{1,-30}{2}", " Description: ", componentInfo.description, Environment.NewLine);
                output.AppendFormat("{0,-30}{1,-30}{2}", " Author: ", componentInfo.author, Environment.NewLine);
                output.AppendFormat("{0,-30}{1,-30}{2}", " SampleImage: ", componentInfo.sampleImage, Environment.NewLine);
                output.AppendFormat("{0,-30}{1,-30}{2}", " backupSampleImage: ", componentInfo.backupSampleImage, Environment.NewLine);
                output.AppendLine();
            }

            output.AppendFormat("{0,-30}{1,-30}{2}", "GUID: ", groupManager.guid, Environment.NewLine);
            output.AppendFormat("{0,-30}{1,-30}{2}", "backgroundImage: ", groupManager.backgroundImage, Environment.NewLine);
            output.AppendFormat("{0,-30}{1,-30}{2}", "pakBackgroundImage: ", groupManager.pakBackgroundImage, Environment.NewLine);
            output.AppendFormat("{0,-30}{1,-30}{2}", "backgroundImageDisabled: ", groupManager.backgroundImageDisable, Environment.NewLine);
            output.AppendLine();

            var achievementList = groupManager.achievementList;
            if (achievementList == null)
            {
                output.AppendLine("achievementList is null, no achievements to print");
                return output.ToString();
            }

            output.AppendLine("Achievement List:");
            var count = 0;
            foreach (var achievement in achievementList)
            {
                //TODO: remove restriction, or build it properly
                if (count == 5) break;
                count++;

                if (achievement.Key == null)
                {
                    output.AppendLine("Achievement.Key is null"); 
                    continue;
                }
                if (achievement.Value == null)
                {
                    output.AppendLine("Achievement.Value is null");
                    continue;
                }
                
                output.AppendFormat("{0,-30}{1,-30}{2}", "Key: ",  achievement.Key, Environment.NewLine);
                output.AppendFormat("{0,-30}{1,-30}{2}", "  fileSoundPath: ", achievement.Value.fileSoundPath, Environment.NewLine);
                output.AppendFormat("{0,-30}{1,-30}{2}", "  pakSoundPath: ", achievement.Value.pakSoundPath, Environment.NewLine);
                output.AppendFormat("{0,-30}{1,-30}{2}", " dynamicSounds: ", achievement.Value.dynamicSounds, Environment.NewLine);

                if (achievement.Value.dynamicSounds == null) continue;

                var sounds = achievement.Value.dynamicSounds.sounds;
                if (sounds == null) continue;
                foreach (var sound in sounds)
                {
                    output.AppendFormat("{0,-30}{1,-30}{2}", "  soundfile: ", sound.soundFile, Environment.NewLine);
                    output.AppendFormat("{0,-30}{1,-30}{2}", "  pakSoundFile: ", sound.pakSoundFile, Environment.NewLine);
                }

                output.AppendFormat("{0,-32}{1,-30}{2}", "imageEnabledInGame: " , achievement.Value.imageEnabledInGame, Environment.NewLine);
                output.AppendFormat("{0,-30}{1,-30}{2}", "imageEnabledStreaming: ", achievement.Value.imageEnabledStreaming, Environment.NewLine);
                output.AppendFormat("{0,-30}{1,-30}{2}", "soundEnabled: ", achievement.Value.soundEnabled, Environment.NewLine);
            }

            output.AppendFormat("{0}ComponentData: {1} entries {0}", Environment.NewLine, VoicePack.componentData.Count);
            count = 0;
            foreach (var dataPair in VoicePack.componentData)
            {
                if (count == 5) break;
                count++;

                output.AppendFormat("{0,-30}{1,-30}{2}", "Key: ", dataPair.Key, Environment.NewLine);
            }

            return output.ToString();
        }
    }
}
