using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using RecursionTracker.Plugins.VoicePackCombiner.Properties.Annotations;
using RecursionTracker.Plugins.VoicePackCombiner.VoicePack;

namespace RecursionTracker.Plugins.VoicePackCombiner
{
    /// <summary> 
    /// Represents the main model/data of the plugin  
    /// </summary>
    public class VoicePackCombiner : INotifyPropertyChanged
    {
        /// <summary>
        /// List of VoicePacks to combine, allows for databinding with GUI controls
        /// using FileInfo as type rather than the full path string to be able to bind the name property easily
        /// </summary>
        public BindingList<FileInfo> VoicePackFiles { get; set; } = new BindingList<FileInfo>();

        /// <summary>
        /// Holds the merged combination of all the added voicepacks
        /// </summary>
        public VoicePackExtended CombinedVoicePack { get; private set; }

        /// <summary>
        /// Holds a reference to the GlobalVariablesPS2.achievementOptions, so it can be restored when needed
        /// </summary>
        public VoicePackExtended OriginalAchievementsOptionsBackup { get; private set; }

        /// <summary>
        /// VoicePackCombiner uses this filename to save the current combined voicepack to disk
        /// </summary>
        readonly static string CombinedVoicePackBackupFilename = Path.Combine(Application.StartupPath, "VoicePackCombiner.CurrentCombinedVoicePack" + PlanetSide2.GlobalVariablesPS2.VOICEPACK_FILE_EXT);

        /// <summary>
        /// This property switches between using the combined voice pack and the original voice pack loaded in the main program
        /// Saves itself as a setting every call
        /// It emits a UseCombinedVoicePack property changed event
        ///  </summary>
        public bool UseCombinedVoicePack
        {
            get { return _useCombinedVoicePack; }

            set
            {
                var useCombinedVoicePack = value;
                if (useCombinedVoicePack)
                {
                    if (!CombinedVoicePack.IsValidVoicePackLoaded())
                    {
                        //Can't use non-existing voicepack so dont change value, 
                        //do send out changed event so ui element used to set this to true, knows the value isn't accepted and the gui element is unset again 
                        Debug.WriteLine("useCombinedVoicepack: " + _useCombinedVoicePack);
                        OnPropertyChanged("UseCombinedVoicePack");
                        return;
                    }

                    OriginalAchievementsOptionsBackup.GetFromGlobal();
                    CombinedVoicePack.SetAsGlobal();
                }
                else
                {
                    //var globalAchievmentOptions = new VoicePackExtended();
                    //globalAchievmentOptions.GetFromGlobal();
                    //if (globalAchievmentOptions.EqualSoundFilenames(CombinedVoicePack))
                    if(CombinedVoicePack.IsGlobal())
                        //The voicepack in use is indeed our combined one, so revert to the backup voicepack
                        OriginalAchievementsOptionsBackup.SetAsGlobal();
                    else
                    {
                        //The voicepack in use is not the combined one, meaning the user has used the main program to load another voicepack
                        //while useCombinedVoicePack==true, so the backup will be out of date. We create a new backup to get back in sync.
                        OriginalAchievementsOptionsBackup.GetFromGlobal();
                    }
                }
                _useCombinedVoicePack = value;
                OnPropertyChanged("UseCombinedVoicePack");

                UpdateUseCombinedVoicePackSettingsFile();
            }
        }

        private bool _useCombinedVoicePack = false;

        private void UpdateUseCombinedVoicePackSettingsFile()
        {
            Properties.VoicePackCombiner.Default.UseCombinedVoicePack = _useCombinedVoicePack;
            Properties.VoicePackCombiner.Default.Save();
        }


        public VoicePackCombiner(bool loadFromSettingFile = true)
        {
            CombinedVoicePack = new VoicePackExtended();
            OriginalAchievementsOptionsBackup = new VoicePackExtended();
            if(loadFromSettingFile) LoadFromUserSettings(); 
        }

        /// <summary>
        /// Loads the last saved state from the user's settings file
        /// </summary>
        void LoadFromUserSettings()
        {
            LoadVoicePackFileListFromSettings();
            UseCombinedVoicePack = Properties.VoicePackCombiner.Default.UseCombinedVoicePack;
        }

        /// <summary>
        /// Loads the last used list of voicepacks (to combine) from the user settings file
        /// </summary>
        private void LoadVoicePackFileListFromSettings()
        {
            //TODO: this was once an issue because I messed up the settings file creation, afaik this cant be null as it always has a default value?
            if(Properties.VoicePackCombiner.Default.VoicePackFileList == null) return;

            CombinedVoicePack = new VoicePackExtended();
            AddVoicePacks(Properties.VoicePackCombiner.Default.VoicePackFileList.Cast<string>().ToList());
        }

        /// <summary>
        /// Save the current list of voicepacks (to combine) to the user settings file
        /// </summary>
        void SaveVoicePackFileListToSettings()
        {
            Properties.VoicePackCombiner.Default.VoicePackFileList = new StringCollection();
            var voicePackSettings = Properties.VoicePackCombiner.Default.VoicePackFileList;
            foreach (var voicePackFile in VoicePackFiles)
            {
                voicePackSettings.Add(voicePackFile.FullName);
            }
            Properties.VoicePackCombiner.Default.Save();
        }

        /// <summary>
        /// Adds and merges in the list of filenames into the current voicepak list.
        /// </summary>
        public void AddVoicePacks(List<string> filenames)
        {
            foreach (var filename in filenames)
            {
                bool loadSuccess = false;

                if (!VoicePackFiles.Any())
                    loadSuccess = CombinedVoicePack.LoadFromFile(filename);
                else
                    loadSuccess = CombinedVoicePack.Merge(filename);

                if(loadSuccess)
                    VoicePackFiles.Add(new FileInfo(filename));
                else
                    throw new FileLoadException("Failed to load voicepack", filename);
            }
            SaveVoicePackFileListToSettings();
        }

        /// <summary>
        /// Removes the list of files from the current voicepack list
        /// </summary>
        /// <remarks>
        /// In stead of actually removing, I just recreate the combined voicepack from the updated list.
        /// My initial tests seem to indicate this is a pretty fast operation and seems stable. 
        /// To actually create a real remove, much work is needed.
        /// </remarks>
        public void RemoveVoicePacks(List<FileInfo> files)
        {
            foreach (var file in files) VoicePackFiles.Remove(file);

            ReCombine();

            SaveVoicePackFileListToSettings();
        }

        /// <summary>
        /// Rebuilds _combinedAchievementOptions from the VoicePackFiles list
        /// It rereads the voicepacks from file, so checks if they still load and removes from list when necessary
        ///  </summary>
        private void ReCombine()
        {
            if (!VoicePackFiles.Any())
            {
                CombinedVoicePack.VoicePack = null;
                return;
            }

            CombinedVoicePack = new VoicePackExtended();
            CombinedVoicePack.LoadFromFile(VoicePackFiles[0].FullName);
            List<FileInfo> invalidFilesToRemove = new List<FileInfo>();
            foreach (var voicePackFile in VoicePackFiles.Skip(1))
            {
                if(!CombinedVoicePack.Merge(voicePackFile.FullName))
                    invalidFilesToRemove.Add(voicePackFile);
            }

            foreach (var invalidFile in invalidFilesToRemove)
            {
                VoicePackFiles.Remove(invalidFile);
            }
        }


        /// <summary>
        /// Checks if the current loaded voicepack aka globalVoicePack is changed without using this class/plugin (for example using the main program to load a new voicepack)
        /// If it is changed, update the internal state to reflect the change.
        /// </summary>
        /// <returns>true if it was changed, false if they where equal</returns>
        public bool CheckGlobalVoicePackChanged()
        {
            //This makes only sense when the combined voicepack is (supposed to be) in use
            if (!UseCombinedVoicePack) return false;

            if (CombinedVoicePack.IsGlobal())
            {
                //Debug.WriteLine("Global VoicePack not changed");
                return false;
            }
            else
            {
                //Debug.WriteLine("Global VoicePack changed");
                OriginalAchievementsOptionsBackup.GetFromGlobal();  
                UseCombinedVoicePack = false;
                return true;
            }
        }

#region INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
#endregion

    }
}
