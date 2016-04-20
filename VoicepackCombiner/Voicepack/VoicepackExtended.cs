using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using RecursionTracker.Plugins.PlanetSide2;

namespace RecursionTracker.Plugins.VoicepackCombiner.Voicepack
{
    /// <summary>
    /// VoicepackExtended represents an extension to AchievementOptionsComponents (which is ao. the in memory representation of Voicepacks)
    /// When loading/saving voicepack files from/to a file using AchievementOptionsComponents,
    /// some global variables get written so that the loaded file is always used as the current voicepack.
    /// This is a wrapper class to decouple the loading of the voicepack from a file, 
    /// from the actual using of the voicepack by the main program.
    /// 
    /// VoicepackExtended also functions as a facade to VoicepackMerger and VoicepackComparer.
    /// </summary>
    /// 
    /// <remarks>
    /// I use the name "Voicepack" in stead of the "AchievmentOptionsComponents". 
    /// "AchievmentOptions" is a separate class, represents one achievement, and is used in the AchievmentList (dictionary/map).
    /// However the name achievementOptions is also found in GlobalVariablesPS2.achievementOptions for example, but there it is
    /// actually a AchievmentOptionsComponents instance. 
    /// </remarks>
    public class VoicepackExtended
    {
        /// Global Variables to take care/keep track of:
        /// GlobalVariablesPS2.achievementOptions: the voicepack instance that gets used by the main program, gets changed during loading/saving
        /// GlobalVariablesPS2.loadedVoicePack: gets set when loading from a PAK based voicepack file
        /// GlobalVariablesPS2.loadedVoicePackConfigFile: points to the xml based voicepack file
        /// GlobalVariablesPS2.usingPAK: boolean that indicates if the voicepack is loaded from a pak, or xml based voicepack file

        public AchievementOptionsComponents Voicepack { get; set; } = null;
        public string LoadedVoicepack { get; set; }
        public string LoadedVoicepackConfigFile { get; set; }
        public bool UsingPAK { get; set; }

        /// <summary>
        /// Creates a default soundpack from null
        /// </summary>
        public void InitializeToDefault()
        {
            Voicepack = new AchievementOptionsComponents();
            Voicepack.InitializeOnNull(); 
            Voicepack.RestoreDefaults(); 
        }

        /// <summary>
        /// Creates a reference to the global AchievementOptions and all of the global state affected by loading/saving
        /// the achievementOptions(Components)
        /// </summary>
        public void GetFromGlobal()
        {
            Voicepack = GlobalVariablesPS2.achievementOptions;
            LoadedVoicepack = GlobalVariablesPS2.loadedVoicePack;
            LoadedVoicepackConfigFile = GlobalVariablesPS2.loadedVoicePackConfigFile;
            UsingPAK = GlobalVariablesPS2.usingPAK;
        }

        /// <summary>
        /// Sets this voicepack as the GlobalVariablesPS2.achievementOptions, which causes this.Voicepack to be used by the main program.
        /// </summary>
        public void SetAsGlobal()
        {
            GlobalVariablesPS2.achievementOptions = Voicepack;
            GlobalVariablesPS2.loadedVoicePack = LoadedVoicepack;
            GlobalVariablesPS2.loadedVoicePackConfigFile = LoadedVoicepackConfigFile;
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
            return GlobalVariablesPS2.loadedVoicePack == this.LoadedVoicepack
                   && GlobalVariablesPS2.loadedVoicePackConfigFile == this.LoadedVoicepackConfigFile;
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
            var globalBackup = new VoicepackExtended();

            globalBackup.GetFromGlobal();
            {
                try
                {
                    GlobalVariablesPS2.achievementOptions = new AchievementOptionsComponents();
                    GlobalVariablesPS2.achievementOptions.LoadFromPAKFile(filename);
                    this.GetFromGlobal();
                }
                catch (Exception e)
                {
                    MessageBox.Show($"Failed to load:\n{filename}\n\n With error:\n{e}");
                }
            }
            globalBackup.SetAsGlobal();

            return IsValidVoicepackLoaded();
        }

        /// <summary>
        /// Returns true if this contains a valid voicepack, i.e. a voicepack that contains something rather than nothing
        /// </summary>
        /// <returns></returns>
        public bool IsValidVoicepackLoaded()
        {
            //This is not a complete validation, but trail and error has shown this to be enough
            return Voicepack?.groupManager?.achievementList != null 
                && Voicepack.groupManager.achievementList.Count != 0;
        }

        /// <summary>
        /// Saves this.Voicepack to a binary voicepack file. Makes sure any global state that might be altered during this
        /// operation is restored.
        /// </summary>
        /// <remarks> see remarks LoadFromFile() </remarks>
        public void ExportToFile(string filename)
        {
            if (!IsValidVoicepackLoaded()) return;

            var globalBackup = new VoicepackExtended();
            globalBackup.GetFromGlobal();
            {
                this.SetAsGlobal();
                this.Voicepack.CreatePAKFile(filename);
                this.GetFromGlobal(); //make sure the global friends are copied
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
            var other = new VoicepackExtended();
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
        public void Merge(VoicepackExtended other)
        {
            if(!IsValidVoicepackLoaded())
                throw new InvalidOperationException();
            if(other == null || !other.IsValidVoicepackLoaded())
                throw new ArgumentNullException();

            VoicepackMerger.Merge(this, other);
        }
#endregion

#region equals functions
        /// <summary>
        /// Checks if this.voicepack refers to the same sounds as other.voicepack
        /// </summary>
        /// <returns>true when equal, false when not equal</returns>
        public bool EqualSoundFilenames(VoicepackExtended other)
        {
            if (!this.IsValidVoicepackLoaded() && !other.IsValidVoicepackLoaded())
                return true;

            if (!this.IsValidVoicepackLoaded() || !other.IsValidVoicepackLoaded())
                return false;

            var achievements = Voicepack.groupManager.achievementList;
            var otherAchievements = other.Voicepack.groupManager.achievementList;

            return VoicepackComparer.EqualAchievementLists(achievements, otherAchievements);
        }

        public bool EqualComponentInfo(VoicepackExtended other)
        {
            if (Voicepack?.componentInformation == null && other.Voicepack?.componentInformation == null)
                return true;
            if (Voicepack?.componentInformation == null || other.Voicepack?.componentInformation == null)
                return false;

            return VoicepackComparer.EqualComponentInformation(Voicepack.componentInformation,
                other.Voicepack.componentInformation);
        }
#endregion

        /// <summary>
        /// Facade function, removes any stored data that is not referenced
        /// </summary>
        public List<string> RemoveUnusedComponentData()
        {
            return VoicepackCleaner.RemoveUnusedComponentData(this);
        }

        /// <summary>
        /// Produces a formatted string representing the contents of this instance mainly used for debugging purposes.
        /// TODO: refactor into its own class, and remove some code duplication
        /// </summary>
        /// <returns></returns>
        public string ToString()
        {
            if (Voicepack == null) return "Voicepack == null"; 

            var output = new StringBuilder();

            output.AppendLine();
            output.AppendFormat("{0,-30}{1,-30}{2}", "LoadedVoicepack: ", LoadedVoicepack, Environment.NewLine);
            output.AppendFormat("{0,-30}{1,-30}{2}", "LoadedVoicepackConfigFile: ", LoadedVoicepackConfigFile, Environment.NewLine);
            output.AppendFormat("{0,-30}{1,-30}{2}", "UsingPAK: ", UsingPAK, Environment.NewLine);

            var groupManager = Voicepack.groupManager;
            if (groupManager == null)
            {
                output.AppendLine("groupManager is null, nothing further to print");
                return output.ToString();
            }

            var componentInfo = groupManager.componentInfo;
            if (Voicepack.groupManager.componentInfo == null)
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

            output.AppendFormat("{0}ComponentData: {1} entries {0}", Environment.NewLine, Voicepack.componentData.Count);
            count = 0;
            foreach (var dataPair in Voicepack.componentData)
            {
                if (count == 5) break;
                count++;

                output.AppendFormat("{0,-30}{1,-30}{2}", "Key: ", dataPair.Key, Environment.NewLine);
            }

            return output.ToString();
        }
    }
}
