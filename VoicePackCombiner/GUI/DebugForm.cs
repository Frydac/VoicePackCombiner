using System;
using System.Windows.Forms;
using RecursionTracker.Plugins.VoicePackCombiner.VoicePack;

namespace RecursionTracker.Plugins.VoicePackCombiner.GUI
{
    public partial class DebugForm : Form
    {
        private VoicePackCombiner _voicePackCombiner;
        //private TestDataBinding _test;
        public DebugForm(VoicePackCombiner voicePackCombiner)
        {
            InitializeComponent();

            _voicePackCombiner = voicePackCombiner;
            //TestDataBinding _test = new TestDataBinding();
            ////checkBox1.DataBindings.Add("Checked", _test, "TestDataBindingProperty");
            //checkBox1.DataBindings.Add(new Binding("Checked", _test, "TestDataBindingProperty", true,
            //    DataSourceUpdateMode.OnPropertyChanged));
        }

        private void btnPrintGlobalVoicePack_Click(object sender, EventArgs e)
        {
            //VoicePackCombiner.PrintToConsole(GlobalVariablesPS2.achievementOptions);
            VoicePackExtended globalCopyToPrint = new VoicePackExtended();
            globalCopyToPrint.GetFromGlobal();
            this.txtBoxDebugInfo.AppendText(globalCopyToPrint.ToString());
        }

        private void btnPrintCombinedVoicePack_Click(object sender, EventArgs e)
        {
            this.txtBoxDebugInfo.AppendText(_voicePackCombiner.CombinedVoicePack.ToString());
        }

        private void btnPrintBackupVoicePack_Click(object sender, EventArgs e)
        {
            this.txtBoxDebugInfo.AppendText(_voicePackCombiner.OriginalAchievementsOptionsBackup.ToString());
        }

        private void btnClearTxtBox_Click(object sender, EventArgs e)
        {
            this.txtBoxDebugInfo.Clear();
        }
    }
}
