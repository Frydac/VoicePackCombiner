using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RecursionTracker.Plugins.VoicePackCombiner.GUI
{
    /// <summary>
    /// Represents the Menu Item that is added to the main gui form in order for the user
    /// to access the VoicePackCombiner plugin
    /// It serves as view and controller using VoicePackCombiner as model
    /// </summary>
    internal class VoicePackCombinerMenuItems
    {
        private static Color menuForeColor = Color.FromArgb(175, 175, 175);

        /// <summary>
        /// Parent menu item that contains all other menu items for the 
        /// voicepackcombiner plugin. This item gets inserted under the
        /// menuitem passed to the constructor.
        /// </summary>
        private ToolStripMenuItem _voicePackCombinerMenuItem = new ToolStripMenuItem
        {
            Text = "Voice Pack Combiner",
            Name = "_voicePackCombinerItem",
            ForeColor = menuForeColor 
        };

        private ToolStripMenuItem _useCombinedVoicepackMenuItem = new ToolStripMenuItem
        {
            Text = "Use Combined Voice Pack",
            Name = "_useCombinedVoicepackMenuItem",
            CheckOnClick = true,
            ForeColor = menuForeColor,
            Enabled = false  //only enable when there is a combined voicepack (not empty/null)
        };

        private ToolStripMenuItem _settingsMenuItem = new ToolStripMenuItem
        {
            Text = "Settings...",
            ForeColor = menuForeColor,
            Name = "_settingsMenuItem"
        };

        private VoicePackCombiner _voicePackCombiner = null;
        private VoicePackCombinerForm _voicePackCombinerForm = null;
        private GUIMain _mainForm = null;

        /// <summary>
        /// Creates and adds the voicepack combiner menu items as children to parentItemName
        /// </summary>
        public VoicePackCombinerMenuItems(string parentItemName, 
            VoicePackCombiner voicePackCombiner,
            VoicePackCombinerForm voicePackCombinerForm,
            GUIMain mainForm)
        {
            _voicePackCombiner = voicePackCombiner;
            _voicePackCombinerForm = voicePackCombinerForm;
            _mainForm = mainForm;

            CreateVoicePackCombinerSubMenu();
            AddVoicePackCombinerSubMenuToParent(parentItemName);

            UpdateGUIVoicePackListIsEmpty();
        }

        private void CreateVoicePackCombinerSubMenu()
        {
            //useCombinedVoicePack can be changed in the pluginform as well as in this menu, so needs an observer
            //Manual two way binding, as ToolStripMenuItem.checked doesnt implement the databinding features
            _voicePackCombiner.PropertyChanged += useCombinedVoicePack_Changed;
            _useCombinedVoicepackMenuItem.Checked = _voicePackCombiner.UseCombinedVoicePack;

            _useCombinedVoicepackMenuItem.Click += useCombinedVoicePackMenuItem_Click;
            _voicePackCombinerMenuItem.DropDownItems.Add(_useCombinedVoicepackMenuItem);

            _settingsMenuItem.Click += settingsMenuItem_Click;
            _voicePackCombinerMenuItem.DropDownItems.Add(_settingsMenuItem);

            _voicePackCombiner.VoicePacksFilesToCombine.ListChanged += VoicePackFiles_ListChanged;
        }

        public void AddVoicePackCombinerSubMenuToParent(string parentMenuItemName)
        {
            var mainFormMenuStripItems = _mainForm.MainMenuStrip.Items;
            if (mainFormMenuStripItems.ContainsKey(parentMenuItemName))
            {
                var parentMenuItem = mainFormMenuStripItems.Find(parentMenuItemName, false)[0] as ToolStripMenuItem;
                parentMenuItem.DropDownItems.Add(_voicePackCombinerMenuItem);
            }
            else
            {
                throw new InvalidOperationException(
                    "Can't insert new menu item for VoicePackCombiner Plugin. ");
            }
        }

        /// <summary>
        /// Handeling the model/property update because the UI element was used
        /// </summary>
        private void useCombinedVoicePackMenuItem_Click(Object sender, EventArgs e)
        {
            _voicePackCombiner.UseCombinedVoicePack = ((ToolStripMenuItem)sender).Checked;
        }

        /// <summary>
        /// Handeling the UI update because the model/property has changed (menuItem.checked has no databindings)
        /// </summary>
        private void useCombinedVoicePack_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName ==  "UseCombinedVoicePack")
            {
                //_useCombinedVoicepackMenuItem.Checked = ((VoicePackCombiner) sender).UseCombinedVoicePack;
                SetUseCombinedVoicePackMenuItemChecked(((VoicePackCombiner)sender).UseCombinedVoicePack);
            }
        }

        /// <summary>
        /// Helper function along the lines of:
        /// https://msdn.microsoft.com/en-us/library/ms171728.aspx
        /// As there is a timer (aka thread) that can alter UseCombinedVoicePack, this makes it threadsafe 
        /// </summary>
        private void SetUseCombinedVoicePackMenuItemChecked(bool check)
        {
            if (_useCombinedVoicepackMenuItem.GetCurrentParent().InvokeRequired)
            {
                var f = new SetUseCombinedVoicePackMenuItemCheckedCallback(SetUseCombinedVoicePackMenuItemChecked);
                _useCombinedVoicepackMenuItem.GetCurrentParent().Invoke(f, new object[] {check});
            }
            else
                _useCombinedVoicepackMenuItem.Checked = check;
        }
        //Callback declaration to Invoke by this thread
        private delegate void SetUseCombinedVoicePackMenuItemCheckedCallback(bool check);

        /// <summary>
        /// Eventhandler that brings up the VoicePackCombinerForm
        /// </summary>
        private void settingsMenuItem_Click(Object sender, EventArgs e)
        {
            //When the form is closed it is not null, but I have to create a new one in stead of just making it visible
            //because else the rounded edges wont look properly. (Maybe there is a better way to re-initialize the edges..)
            if (_voicePackCombinerForm == null || !_voicePackCombinerForm.Visible)
                _voicePackCombinerForm = new VoicePackCombinerForm(_voicePackCombiner);
            else if (_voicePackCombinerForm.Visible)
                return;

            //Show the plugin GUI and position it in the middle of the main GUI 
            _voicePackCombinerForm.StartPosition = FormStartPosition.Manual;
            _voicePackCombinerForm.Location = new Point(
                _mainForm.Location.X + (_mainForm.Width - _voicePackCombinerForm.Width) / 2,
                _mainForm.Location.Y + (_mainForm.Height - _voicePackCombinerForm.Height) / 2);
            _voicePackCombinerForm.Show(_mainForm);
        }

        /// <summary>
        /// Handeling voice pack list change events, when empty -> disable gui element
        /// </summary>
        void VoicePackFiles_ListChanged(object sender, ListChangedEventArgs e)
        {
            UpdateGUIVoicePackListIsEmpty();
        }

        /// <summary>
        /// Enables/disables menu item depending on wether the voicepackfilelist is empty.
        /// </summary>
        private void UpdateGUIVoicePackListIsEmpty()
        {
            bool enableGUIControl = _voicePackCombiner.VoicePacksFilesToCombine.Any();

            _useCombinedVoicepackMenuItem.Enabled = enableGUIControl;
        }
    }
}
