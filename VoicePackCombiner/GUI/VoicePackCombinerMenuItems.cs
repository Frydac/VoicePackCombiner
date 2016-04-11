using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace RecursionTracker.Plugins.VoicepackCombiner.GUI
{
    /// <summary>
    /// Represents the Menu Item that is added to the main gui form in order for the user
    /// to access the VoicepackCombiner plugin
    /// It serves as view and controller using VoicepackCombiner as model
    /// </summary>
    internal class VoicepackCombinerMenuItems
    {
        private static Color menuForeColor = Color.FromArgb(175, 175, 175);

        /// <summary>
        /// Parent menu item that contains all other menu items for the 
        /// voicepackcombiner plugin. This item gets inserted under the
        /// menuitem passed to the constructor.
        /// </summary>
        private ToolStripMenuItem _voicePackCombinerMenuItem = new ToolStripMenuItem
        {
            Text = "Voicepack Combiner",
            Name = "_voicePackCombinerItem",
            ForeColor = menuForeColor 
        };

        private ToolStripMenuItem _useCombinedVoicepackMenuItem = new ToolStripMenuItem
        {
            Text = "Use Combined Voicepack",
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

        private VoicepackCombiner _voicepackCombiner = null;
        private VoicepackCombinerForm _voicepackCombinerForm = null;
        private GUIMain _mainForm = null;

        /// <summary>
        /// Creates and adds the voicepack combiner menu items as children to parentItemName
        /// </summary>
        public VoicepackCombinerMenuItems(string parentItemName, 
            VoicepackCombiner voicepackCombiner,
            VoicepackCombinerForm voicepackCombinerForm,
            GUIMain mainForm)
        {
            _voicepackCombiner = voicepackCombiner;
            _voicepackCombinerForm = voicepackCombinerForm;
            _mainForm = mainForm;

            CreateVoicepackCombinerSubMenu();
            AddVoicepackCombinerSubMenuToParent(parentItemName);

            UpdateGUIVoicepackListIsEmpty();
        }

        private void CreateVoicepackCombinerSubMenu()
        {
            //useCombinedVoicepack can be changed in the pluginform as well as in this menu, so needs an observer
            //Manual two way binding, as ToolStripMenuItem.checked doesnt implement the databinding features
            _voicepackCombiner.PropertyChanged += UseCombinedVoicepackChanged;
            _useCombinedVoicepackMenuItem.Checked = _voicepackCombiner.UseCombinedVoicepack;

            _useCombinedVoicepackMenuItem.Click += useCombinedVoicepackMenuItem_Click;
            _voicePackCombinerMenuItem.DropDownItems.Add(_useCombinedVoicepackMenuItem);

            _settingsMenuItem.Click += settingsMenuItem_Click;
            _voicePackCombinerMenuItem.DropDownItems.Add(_settingsMenuItem);

            _voicepackCombiner.VoicepacksFilesToCombine.ListChanged += VoicepackFiles_ListChanged;
        }

        public void AddVoicepackCombinerSubMenuToParent(string parentMenuItemName)
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
                    "Can't insert new menu item for VoicepackCombiner Plugin. ");
            }
        }

        /// <summary>
        /// Handeling the model/property update because the UI element was used
        /// </summary>
        private void useCombinedVoicepackMenuItem_Click(Object sender, EventArgs e)
        {
            _voicepackCombiner.UseCombinedVoicepack = ((ToolStripMenuItem)sender).Checked;
        }

        /// <summary>
        /// Handeling the UI update because the model/property has changed (menuItem.checked has no databindings)
        /// </summary>
        private void UseCombinedVoicepackChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName ==  "UseCombinedVoicepack")
            {
                //_useCombinedVoicepackMenuItem.Checked = ((VoicepackCombiner) sender).UseCombinedVoicepack;
                SetUseCombinedVoicepackMenuItemChecked(((VoicepackCombiner)sender).UseCombinedVoicepack);
            }
        }

        /// <summary>
        /// Helper function along the lines of:
        /// https://msdn.microsoft.com/en-us/library/ms171728.aspx
        /// As there is a timer (aka thread) that can alter UseCombinedVoicepack, this makes it threadsafe 
        /// </summary>
        private void SetUseCombinedVoicepackMenuItemChecked(bool check)
        {
            if (_useCombinedVoicepackMenuItem.GetCurrentParent().InvokeRequired)
            {
                var f = new SetUseCombinedVoicepackMenuItemCheckedCallback(SetUseCombinedVoicepackMenuItemChecked);
                _useCombinedVoicepackMenuItem.GetCurrentParent().Invoke(f, new object[] {check});
            }
            else
                _useCombinedVoicepackMenuItem.Checked = check;
        }
        //Callback declaration to Invoke by this thread
        private delegate void SetUseCombinedVoicepackMenuItemCheckedCallback(bool check);

        /// <summary>
        /// Eventhandler that brings up the VoicepackCombinerForm
        /// </summary>
        private void settingsMenuItem_Click(Object sender, EventArgs e)
        {
            //When the form is closed it is not null, but I have to create a new one in stead of just making it visible
            //because else the rounded edges wont look properly. (Maybe there is a better way to re-initialize the edges..)
            if (_voicepackCombinerForm == null || !_voicepackCombinerForm.Visible)
                _voicepackCombinerForm = new VoicepackCombinerForm(_voicepackCombiner);
            else if (_voicepackCombinerForm.Visible)
                return;

            //Show the plugin GUI and position it in the middle of the main GUI 
            _voicepackCombinerForm.StartPosition = FormStartPosition.Manual;
            _voicepackCombinerForm.Location = new Point(
                _mainForm.Location.X + (_mainForm.Width - _voicepackCombinerForm.Width) / 2,
                _mainForm.Location.Y + (_mainForm.Height - _voicepackCombinerForm.Height) / 2);
            _voicepackCombinerForm.Show(_mainForm);
        }

        /// <summary>
        /// Handeling voicepack list change events, when empty -> disable gui element
        /// </summary>
        void VoicepackFiles_ListChanged(object sender, ListChangedEventArgs e)
        {
            UpdateGUIVoicepackListIsEmpty();
        }

        /// <summary>
        /// Enables/disables menu item depending on wether the voicepackfilelist is empty.
        /// </summary>
        private void UpdateGUIVoicepackListIsEmpty()
        {
            bool enableGUIControl = _voicepackCombiner.VoicepacksFilesToCombine.Any();

            _useCombinedVoicepackMenuItem.Enabled = enableGUIControl;
        }
    }
}
