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
    /// Represents the main model/data of the VoicePackCombiner plugin  
    /// </summary>
    public class VoicePackCombiner : INotifyPropertyChanged
    {
        /// <summary>
        /// List of VoicePacks to combine, allows for databinding with GUI controls
        /// using FileInfo as type rather than the full path string to be able to bind the name property easily
        /// </summary>
        public BindingList<FileInfo> VoicePacksFilesToCombine { get; set; } = new BindingList<FileInfo>();

        /// <summary>
        /// Holds the merged combination of all the added voicepacks
        /// </summary>
        public VoicePackExtended CombinedVoicePack { get; private set; }

        /// <summary>
        /// Holds a backup reference to the original GlobalVariablesPS2.achievementOptions before loading VoicePackCombiner
        /// </summary>
        public VoicePackExtended GlobalVoicePackBackup { get; }

        /// <summary>
        /// Holds the filename to save the current combined voicepack to a backup file, this is needed because some
        /// user actions can cause the main program to reload from the file refered to by the current loaded voicepack.
        /// And as this class merges existing voicepacks from different files in memory, the combination also needs to be saved to a file.
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
                    //TODO: this if test is removable when we always load a default voicepack
                    if (!CombinedVoicePack.IsValidVoicePackLoaded())
                    {
                        //Can't use non-existing voicepack so dont change _useCombinedVoicePack, 
                        //do send out changed event so ui element used to set this to true, knows the value isn't accepted and the gui element is unset again 
                        OnPropertyChanged("UseCombinedVoicePack");
                        return;
                    }

                    GlobalVoicePackBackup.GetFromGlobal();
                    CombinedVoicePack.SetAsGlobal();
                }
                else
                {
                    if(CombinedVoicePack.IsGlobal())
                        //The voicepack in use is indeed our combined one, so revert to the backup voicepack
                        GlobalVoicePackBackup.SetAsGlobal();
                    else
                    {
                        //The voicepack in use was not the combined one anymore, meaning the user has used the main program to load another voicepack
                        //while useCombinedVoicePack==true, so the backup will be out of date. We create a new backup to get back in sync.
                        GlobalVoicePackBackup.GetFromGlobal();
                    }
                }
                //accept the input value
                _useCombinedVoicePack = useCombinedVoicePack;
                OnPropertyChanged("UseCombinedVoicePack");

                SaveUseCombinedVoicePackToSettings();
            }
        }

        private bool _useCombinedVoicePack = false;

        public VoicePackCombiner(bool loadFromSettingFile = true)
        {
            CombinedVoicePack = new VoicePackExtended();
            CombinedVoicePack.InitializeToDefault();
            GlobalVoicePackBackup = new VoicePackExtended();
            GlobalVoicePackBackup.InitializeToDefault();
            if(loadFromSettingFile) LoadFromUserSettings(); 
        }

        /// <summary>
        /// Loads the last saved state from the user's settings file
        /// </summary>
        void LoadFromUserSettings()
        {
            LoadVoicePacksToCombineFromSettings();
            UseCombinedVoicePack = Properties.VoicePackCombiner.Default.UseCombinedVoicePack;
        }

        /// <summary>
        /// Loads the last saved list of voicepacks to combine from the user settings file
        /// </summary>
        private void LoadVoicePacksToCombineFromSettings()
        {
            //TODO: this was once an issue because I messed up the settings file creation, afaik this cant be null as it always has a default value?
            if(Properties.VoicePackCombiner.Default.VoicePackFileList == null) return;

            CombinedVoicePack = new VoicePackExtended();
            AddVoicePacks(Properties.VoicePackCombiner.Default.VoicePackFileList.Cast<string>().ToList());
        }

        /// <summary>
        /// Save the current list of voicepacks to combine to the user settings file
        /// </summary>
        void SaveVoicePackFileListToSettings()
        {
            Properties.VoicePackCombiner.Default.VoicePackFileList = new StringCollection();
            var voicePackSettings = Properties.VoicePackCombiner.Default.VoicePackFileList;
            foreach (var voicePackFile in VoicePacksFilesToCombine)
            {
                voicePackSettings.Add(voicePackFile.FullName);
            }
            Properties.VoicePackCombiner.Default.Save();
        }

        private void SaveUseCombinedVoicePackToSettings()
        {
            Properties.VoicePackCombiner.Default.UseCombinedVoicePack = _useCombinedVoicePack;
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

                if (!VoicePacksFilesToCombine.Any())
                    loadSuccess = CombinedVoicePack.LoadFromFile(filename);
                else
                    loadSuccess = CombinedVoicePack.Merge(filename);

                if (loadSuccess)
                    VoicePacksFilesToCombine.Add(new FileInfo(filename));
                else
                    MessageBox.Show($"Failed to load: {filename}\nRemoved from the list of voice packs to combine.");
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
            foreach (var file in files) VoicePacksFilesToCombine.Remove(file);

            RecombineVoicePackFilesToCombine();

            SaveVoicePackFileListToSettings();
        }

        /// <summary>
        /// Rebuilds _combinedAchievementOptions from the VoicePacksFilesToCombine list
        /// It rereads the voicepacks from file, so checks if they still load and removes from list when necessary
        ///  </summary>
        private void RecombineVoicePackFilesToCombine()
        {
            if (!VoicePacksFilesToCombine.Any())
            {
                CombinedVoicePack.VoicePack = null;
                return;
            }

            CombinedVoicePack = new VoicePackExtended();
            CombinedVoicePack.LoadFromFile(VoicePacksFilesToCombine[0].FullName);
            List<FileInfo> invalidFilesToRemove = new List<FileInfo>();
            foreach (var voicePackFile in VoicePacksFilesToCombine.Skip(1))
            {
                if(!CombinedVoicePack.Merge(voicePackFile.FullName))
                    invalidFilesToRemove.Add(voicePackFile);
            }

            foreach (var invalidFile in invalidFilesToRemove)
            {
                VoicePacksFilesToCombine.Remove(invalidFile);
            }
        }


        /// <summary>
        /// Checks if the current loaded voicepack aka globalVoicePack is still the combined voicepack.
        /// The user may have loaded another voicepack using the main program load functionality.
        /// If it is changed, update the internal state to reflect the change.
        /// </summary>
        public void CheckCombinedVoicePackIsStillGlobal()
        {
            //if (!UseCombinedVoicePack) return false;

            //if (CombinedVoicePack.IsGlobal())
            //{
            //    return false;
            //}
            //else
            //{
            //    GlobalVoicePackBackup.GetFromGlobal();  
            //    UseCombinedVoicePack = false;
            //    return true;
            //}

            if (UseCombinedVoicePack && !CombinedVoicePack.IsGlobal())
            {
                UseCombinedVoicePack = false;
                GlobalVoicePackBackup.GetFromGlobal();
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
