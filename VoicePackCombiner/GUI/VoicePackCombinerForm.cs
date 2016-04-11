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

namespace RecursionTracker.Plugins.VoicepackCombiner.GUI
{
    using GlobalVariablesPS2 = PlanetSide2.GlobalVariablesPS2;
    /// <summary>
    /// Represents the main view/GUI/(controller) of the plugin
    /// </summary>
    public partial class VoicepackCombinerForm : GUIForm
    {
        /// <summary>
        /// reference to the main model class which holds all state
        /// </summary>
        VoicepackCombiner _voicepackCombiner;

        /// <summary>
        /// timer used for checking if the current loaded voicepack is changed and update GUI if needed
        /// </summary>
        System.Timers.Timer pollGlobalVoicepackTimer;

        ToolTip VoicepackCombinerFormTooltips = new ToolTip();

        public VoicepackCombinerForm(VoicepackCombiner voicepackCombiner)
        {
            InitializeComponent();
            base.Initialize();

            _voicepackCombiner = voicepackCombiner;

            // Custom renderer for displaying gradients and background color.
            menuStrip1.Renderer = new CustomToolStripRenderer();

            this.RoundedEdges = true;

            listBoxVoicepacks.DataSource = _voicepackCombiner.VoicepacksFilesToCombine;
            listBoxVoicepacks.DisplayMember = "Name";
            listBoxVoicepacks.ItemHeight = 17;
            listBoxVoicepacks.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            listBoxVoicepacks.DrawItem += new DrawItemEventHandler(listBox_DrawItem);
            listBoxVoicepacks.DragEnter += new DragEventHandler(listBox_DragEntered);
            listBoxVoicepacks.DragDrop += new DragEventHandler(listBox_DragDropped);

            //Need to add an updatemethod: otherwise the bound data is only updated when windows is closed.. (now it is updated when property changes, and when windows closes)
            checkBoxEnableCombinedVoicepack.DataBindings.Add("Checked", _voicepackCombiner, "UseCombinedVoicepack", true, DataSourceUpdateMode.OnPropertyChanged);
            _voicepackCombiner.VoicepacksFilesToCombine.ListChanged += VoicepackFiles_ListChanged;
            UpdateGUIVoicepackListIsEmpty();

            //Check if the global voicepack has changed since last time the UI was open
            _voicepackCombiner.CheckCombinedVoicepackIsStillGlobal();
            //Do the check every few seconds while UI is open so it gets updated
            pollGlobalVoicepackTimer = new System.Timers.Timer(5000);
            pollGlobalVoicepackTimer.Elapsed += PollGlobalVoicepackChangedTimerElapsed;
            pollGlobalVoicepackTimer.Start();

            VoicepackCombinerFormTooltips.SetToolTip(listBoxVoicepacks, "Testing tooltips");
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
            listBoxVoicepacks.BackColor = Color.FromArgb(gray, gray, gray);
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
            _voicepackCombiner.AddVoicepacks(files.OfType<string>().ToList());
        }


        /// <summary>
        /// TODO: this should not be in this class!!!
        /// Event handler called by timer to check if the voicepack currently in use was changed (not by the plugin)
        /// </summary>
        void PollGlobalVoicepackChangedTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _voicepackCombiner.CheckCombinedVoicepackIsStillGlobal();
        }
        

        /// <summary>
        /// Event handler for when the voicepacklist of voicepacks to combine is empty or not, it will disable some controls
        /// </summary>
        void VoicepackFiles_ListChanged(object sender, ListChangedEventArgs e)
        {
            Contract.Requires(sender.GetType() == typeof(BindingList<FileInfo>));

            Debug.WriteLine("VoicepackList changed event handler called");

            UpdateGUIVoicepackListIsEmpty();
        }

        /// <summary>
        /// Helper function for VoicepackFiles_ListChanged, extracted method mainly so we can call it in constructor also when the list gets initialized by winforms' databinding.
        /// Will change UI depending on if the list is empty or not
        /// </summary>
        /// <param name="listIsEmpty"></param>
        void UpdateGUIVoicepackListIsEmpty()
        {
            bool enableGUIControl = _voicepackCombiner.VoicepacksFilesToCombine.Any();

            btnRemoveSelected.Enabled = enableGUIControl;
            checkBoxEnableCombinedVoicepack.Enabled = enableGUIControl;
            btnExportCombinedVoicepack.Enabled = enableGUIControl;
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        void btnAddVoicepack_Click(object sender, EventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Add one or more Recursion Stat Tracker Voicepack files",
                Filter = "(*"  + GlobalVariablesPS2.VOICEPACK_FILE_EXT + ")|*" + GlobalVariablesPS2.VOICEPACK_FILE_EXT,
                RestoreDirectory = true,
                Multiselect = true,
                InitialDirectory = Application.StartupPath,
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) 
                return;

            _voicepackCombiner.AddVoicepacks(new List<string>(openFileDialog.FileNames));
        }

        void btnRemoveSelected_Click(object sender, EventArgs e)
        {
            //Make copy of selectedItems list, as it changes when we remove items
            var itemsToRemove = new List<FileInfo>(listBoxVoicepacks.SelectedItems.OfType<FileInfo>().ToList());
            _voicepackCombiner.RemoveVoicepacks(itemsToRemove);
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

        void checkBoxEnableCombinedVoicepack_CheckedChanged(object sender, EventArgs e)
        {
            Debug.WriteLine("checkbox clicked, status: " + ((CheckBox)sender).Checked);
        }

        void btnExportCombinedVoicepack_Click(object sender, EventArgs e)
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

            _voicepackCombiner.CombinedVoicepack.ExportToFile(saveFileDialog.FileName);
        }
    }
}
