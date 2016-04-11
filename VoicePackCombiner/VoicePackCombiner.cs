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
using RecursionTracker.Plugins.VoicepackCombiner.Properties.Annotations;
using RecursionTracker.Plugins.VoicepackCombiner.Voicepack;

namespace RecursionTracker.Plugins.VoicepackCombiner
{
    /// <summary> 
    /// Represents the main model/data of the VoicepackCombiner plugin  
    /// </summary>
    public class VoicepackCombiner : INotifyPropertyChanged
    {
        /// <summary>
        /// List of Voicepacks to combine, allows for databinding with GUI controls
        /// using FileInfo as type rather than the full path string to be able to bind the name property easily
        /// </summary>
        public BindingList<FileInfo> VoicepacksFilesToCombine { get; set; } = new BindingList<FileInfo>();

        /// <summary>
        /// Holds the merged combination of all the added voicepacks
        /// </summary>
        public VoicepackExtended CombinedVoicepack { get; private set; }

        /// <summary>
        /// Holds a backup reference to the original GlobalVariablesPS2.achievementOptions before loading VoicepackCombiner
        /// </summary>
        public VoicepackExtended GlobalVoicepackBackup { get; }

        /// <summary>
        /// Holds the filename to save the current combined voicepack to a backup file, this is needed because some
        /// user actions can cause the main program to reload from the file refered to by the current loaded voicepack.
        /// And as this class merges existing voicepacks from different files in memory, the combination also needs to be saved to a file.
        /// </summary>
        readonly static string CombinedVoicepackBackupFilename = Path.Combine(Application.StartupPath, "VoicepackCombiner.CurrentCombinedVoicepack" + PlanetSide2.GlobalVariablesPS2.VOICEPACK_FILE_EXT);

        /// <summary>
        /// This property switches between using the combined voicepack and the original voicepack loaded in the main program
        /// Saves itself as a setting every call
        /// It emits a UseCombinedVoicepack property changed event
        ///  </summary>
        public bool UseCombinedVoicepack
        {
            get { return _useCombinedVoicepack; }

            set
            {
                var useCombinedVoicepack = value;
                if (useCombinedVoicepack)
                {
                    //TODO: this if test is removable when we always load a default voicepack
                    if (!CombinedVoicepack.IsValidVoicepackLoaded())
                    {
                        //Can't use non-existing voicepack so dont change _useCombinedVoicepack, 
                        //do send out changed event so ui element used to set this to true, knows the value isn't accepted and the gui element is unset again 
                        OnPropertyChanged("UseCombinedVoicepack");
                        return;
                    }

                    GlobalVoicepackBackup.GetFromGlobal();
                    CombinedVoicepack.SetAsGlobal();
                }
                else
                {
                    if(CombinedVoicepack.IsGlobal())
                        //The voicepack in use is indeed our combined one, so revert to the backup voicepack
                        GlobalVoicepackBackup.SetAsGlobal();
                    else
                    {
                        //The voicepack in use was not the combined one anymore, meaning the user has used the main program to load another voicepack
                        //while useCombinedVoicepack==true, so the backup will be out of date. We create a new backup to get back in sync.
                        GlobalVoicepackBackup.GetFromGlobal();
                    }
                }
                //accept the input value
                _useCombinedVoicepack = useCombinedVoicepack;
                OnPropertyChanged("UseCombinedVoicepack");

                SaveUseCombinedVoicepackToSettings();
            }
        }

        private bool _useCombinedVoicepack = false;

        public VoicepackCombiner(bool loadFromSettingFile = true)
        {
            CombinedVoicepack = new VoicepackExtended();
            CombinedVoicepack.InitializeToDefault();
            GlobalVoicepackBackup = new VoicepackExtended();
            GlobalVoicepackBackup.InitializeToDefault();
            if(loadFromSettingFile) LoadFromUserSettings(); 
        }

        /// <summary>
        /// Loads the last saved state from the user's settings file
        /// </summary>
        void LoadFromUserSettings()
        {
            LoadVoicepacksToCombineFromSettings();
            UseCombinedVoicepack = Properties.VoicepackCombiner.Default.UseCombinedVoicepack;
        }

        /// <summary>
        /// Loads the last saved list of voicepacks to combine from the user settings file
        /// </summary>
        private void LoadVoicepacksToCombineFromSettings()
        {
            //TODO: this was once an issue because I messed up the settings file creation, afaik this cant be null as it always has a default value?
            if(Properties.VoicepackCombiner.Default.VoicepackFileList == null) return;

            CombinedVoicepack = new VoicepackExtended();
            AddVoicepacks(Properties.VoicepackCombiner.Default.VoicepackFileList.Cast<string>().ToList());
        }

        /// <summary>
        /// Save the current list of voicepacks to combine to the user settings file
        /// </summary>
        void SaveVoicepackFilesToCombineToSettings()
        {
            Properties.VoicepackCombiner.Default.VoicepackFileList = new StringCollection();
            var voicePackSettings = Properties.VoicepackCombiner.Default.VoicepackFileList;
            foreach (var voicePackFile in VoicepacksFilesToCombine)
            {
                voicePackSettings.Add(voicePackFile.FullName);
            }
            Properties.VoicepackCombiner.Default.Save();
        }

        private void SaveUseCombinedVoicepackToSettings()
        {
            Properties.VoicepackCombiner.Default.UseCombinedVoicepack = _useCombinedVoicepack;
            Properties.VoicepackCombiner.Default.Save();
        }

        /// <summary>
        /// Adds and merges in the list of filenames into the current voicepak list.
        /// </summary>
        public void AddVoicepacks(List<string> filenames)
        {
            if (!VoicepacksFilesToCombine.Any())
            {
                CombinedVoicepack = new VoicepackExtended();
                CombinedVoicepack.InitializeToDefault();
            }

            foreach (var filename in filenames)
            {
                if (CombinedVoicepack.Merge(filename))
                    VoicepacksFilesToCombine.Add(new FileInfo(filename));
            }

            SaveVoicepackFilesToCombineToSettings();
        }

        /// <summary>
        /// Removes the list of files from the current voicepack list
        /// </summary>
        /// <remarks>
        /// In stead of actually removing, I just recreate the combined voicepack from the updated list.
        /// My initial tests seem to indicate this is a fast enough operation and seems stable. 
        /// To actually create a real remove, much work is needed.
        /// </remarks>
        public void RemoveVoicepacks(List<FileInfo> files)
        {
            foreach (var file in files) VoicepacksFilesToCombine.Remove(file);

            RecombineVoicepackFilesToCombine();

            SaveVoicepackFilesToCombineToSettings();
        }

        /// <summary>
        /// Rebuilds CombinedVoicepack from the VoicepacksFilesToCombine list
        /// </summary>
        private void RecombineVoicepackFilesToCombine()
        {
            CombinedVoicepack = new VoicepackExtended();
            CombinedVoicepack.InitializeToDefault();

            List<FileInfo> invalidFilesToRemove = new List<FileInfo>();

            foreach (var voicePackFile in VoicepacksFilesToCombine.Skip(1))
            {
                if(!CombinedVoicepack.Merge(voicePackFile.FullName))
                    invalidFilesToRemove.Add(voicePackFile);
            }

            foreach (var invalidFile in invalidFilesToRemove)
            {
                VoicepacksFilesToCombine.Remove(invalidFile);
            }
        }


        /// <summary>
        /// Checks if the current loaded voicepack aka globalVoicepack is still the combined voicepack.
        /// The user may have loaded another voicepack using the main program load functionality.
        /// If it is changed, update the internal state to reflect the change.
        /// </summary>
        public void CheckCombinedVoicepackIsStillGlobal()
        {
            if (UseCombinedVoicepack && !CombinedVoicepack.IsGlobal())
            {
                UseCombinedVoicepack = false;
                GlobalVoicepackBackup.GetFromGlobal();
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
