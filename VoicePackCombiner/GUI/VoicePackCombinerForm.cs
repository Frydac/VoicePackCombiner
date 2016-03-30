using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using RecursionTracker.Plugins.PlanetSide2;

namespace RecursionTracker.Plugins.VoicePackCombiner.GUI
{
    using GlobalVariablesPS2 = PlanetSide2.GlobalVariablesPS2;
    /// <summary>
    /// Represents the main view/GUI/(controller) of the plugin
    /// </summary>
    public partial class VoicePackCombinerForm : GUIForm
    {
        /// <summary>
        /// reference to the main model class which holds all state
        /// </summary>
        VoicePackCombiner _voicePackCombiner;

        /// <summary>
        /// timer used for checking if the current loaded voicepack is changed and update GUI if needed
        /// </summary>
        System.Timers.Timer pollGlobalVoicePackTimer;

        ToolTip VoicePackCombinerFormTooltips = new ToolTip();

        public VoicePackCombinerForm(VoicePackCombiner voicePackCombiner)
        {
            InitializeComponent();
            base.Initialize();

            _voicePackCombiner = voicePackCombiner;

            // Custom renderer for displaying gradients and background color.
            menuStrip1.Renderer = new CustomToolStripRenderer();

            this.RoundedEdges = true;

            listBoxVoicePacks.DataSource = _voicePackCombiner.VoicePackFiles;
            listBoxVoicePacks.DisplayMember = "Name";
            listBoxVoicePacks.ItemHeight = 17;
            listBoxVoicePacks.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            listBoxVoicePacks.DrawItem += new DrawItemEventHandler(listBox_DrawItem);
            listBoxVoicePacks.DragEnter += new DragEventHandler(listBox_DragEntered);
            listBoxVoicePacks.DragDrop += new DragEventHandler(listBox_DragDropped);

            //Need to add an updatemethod: otherwise the bound data is only updated when windows is closed.. (now it is updated when property changes, and when windows closes)
            checkBoxEnableCombinedVoicePack.DataBindings.Add("Checked", _voicePackCombiner, "UseCombinedVoicePack", true, DataSourceUpdateMode.OnPropertyChanged);
            _voicePackCombiner.VoicePackFiles.ListChanged += VoicePackFiles_ListChanged;
            UpdateGUIVoicePackListIsEmpty();

            //Check if the global voicepack has changed since last time the UI was open
            _voicePackCombiner.CheckGlobalVoicePackChanged();
            //Do the check every few seconds while UI is open so it gets updated
            pollGlobalVoicePackTimer = new System.Timers.Timer(5000);
            pollGlobalVoicePackTimer.Elapsed += pollGlobalVoicePackChanged_timerElapsed;
            pollGlobalVoicePackTimer.Start();

            VoicePackCombinerFormTooltips.SetToolTip(listBoxVoicePacks, "Testing tooltips");
        }

        /// <summary>
        /// Need to override to be able to change some colors
        /// </summary>
        public override void UpdateScheme()
        {
            base.UpdateScheme();

            UpdateColors();
        }

        private void UpdateColors()
        {
            int gray = 64;
            listBoxVoicePacks.BackColor = Color.FromArgb(gray, gray, gray);
        }

        private void listBox_DragEntered(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void listBox_DragDropped(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files)
            {
                Console.WriteLine(file);
            }
            _voicePackCombiner.AddVoicePacks(files.OfType<string>().ToList());
        }


        /// <summary>
        /// TODO: this should not be in this class!!!
        /// Event handler called by timer to check if the voicepack currently in use was changed (not by the plugin)
        /// </summary>
        void pollGlobalVoicePackChanged_timerElapsed(object sender, ElapsedEventArgs e)
        {
            //Debug.WriteLine("time elapsed, eventhandler called");
            _voicePackCombiner.CheckGlobalVoicePackChanged();
        }
        

        /// <summary>
        /// Event handler for when the voicepacklist of voicepacks to combine is empty or not, it will disable some controls
        /// </summary>
        void VoicePackFiles_ListChanged(object sender, ListChangedEventArgs e)
        {
            Contract.Requires(sender.GetType() == typeof(BindingList<FileInfo>));

            Debug.WriteLine("VoicePackList changed event handler called");

            UpdateGUIVoicePackListIsEmpty();
        }

        /// <summary>
        /// Helper function for VoicePackFiles_ListChanged, extracted method mainly so we can call it in constructor also when the list gets initialized by winforms' databinding.
        /// Will change UI depending on if the list is empty or not
        /// </summary>
        /// <param name="listIsEmpty"></param>
        void UpdateGUIVoicePackListIsEmpty()
        {
            bool enableGUIControl = _voicePackCombiner.VoicePackFiles.Any();

            btnRemoveSelected.Enabled = enableGUIControl;
            checkBoxEnableCombinedVoicePack.Enabled = enableGUIControl;
            btnExportCombinedVoicePack.Enabled = enableGUIControl;
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        void btnAddVoicePack_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Add one or more Recursion Stat Tracker VoicePack files",
                Filter = "(*"  + GlobalVariablesPS2.VOICEPACK_FILE_EXT + ")|*" + GlobalVariablesPS2.VOICEPACK_FILE_EXT,
                RestoreDirectory = true,
                Multiselect = true,
                InitialDirectory = Application.StartupPath,
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) 
                return;

            _voicePackCombiner.AddVoicePacks(new List<string>(openFileDialog.FileNames));
        }

        void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            //Make copy of selectedItems list, as it changes when we remove items
            var itemsToRemove = new List<FileInfo>(listBoxVoicePacks.SelectedItems.OfType<FileInfo>().ToList());
            _voicePackCombiner.RemoveVoicePacks(itemsToRemove);
        }

        /// <summary>
        /// Custom drawhandler for listbox, mainly to make item selection highlight slateGray
        /// </summary>
        /// <remarks>
        /// Based on:
        /// https://msdn.microsoft.com/en-us/library/system.windows.forms.drawitemstate.aspx
        /// </remarks>
        void listBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;
            Brush brush = ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                ? Brushes.SlateGray
                : new SolidBrush(e.BackColor);
            g.FillRectangle(brush, e.Bounds);

            if (e.Index == -1) return;  //occurs when list is empty, no need to draw non-existing item
            //Returns the databound/datasource/displaymember Value.
            string displayValue = ((ListBox)sender).GetItemText(((ListBox)sender).Items[e.Index]);
            e.Graphics.DrawString(displayValue, e.Font, new SolidBrush(e.ForeColor), e.Bounds, StringFormat.GenericDefault);
            e.DrawFocusRectangle();
        }

        void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        void checkBoxEnableCombinedVoicePack_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("checkbox clicked, status: " + ((CheckBox)sender).Checked);
        }

        void btnExportCombinedVoicePack_Click(object sender, EventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Title = "Export Combined Voicepack",
                Filter = "(*"  + GlobalVariablesPS2.VOICEPACK_FILE_EXT + ")|*" + GlobalVariablesPS2.VOICEPACK_FILE_EXT,
                RestoreDirectory = true,
                InitialDirectory = Application.StartupPath,
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK) 
                return;

            _voicePackCombiner.CombinedVoicePack.ExportToFile(saveFileDialog.FileName);
        }
    }

    //public class TestDataBinding : INotifyPropertyChanged
    //{
    //    bool _test = true;
    //    public bool TestDataBindingProperty
    //    {
    //        get { return _test; }
    //        set
    //        {
    //            Debug.WriteLine("testDataBindingProperty set() called with value: " + value);
    //            _test = value;
    //            OnPropertyChanged("TestDataBindingProperty");
    //        }
    //    }

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //    {
    //        var handler = PropertyChanged;
    //        if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}
}
