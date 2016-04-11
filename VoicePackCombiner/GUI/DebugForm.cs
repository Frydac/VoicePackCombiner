using System;
using System.Windows.Forms;
using RecursionTracker.Plugins.VoicepackCombiner.Voicepack;

namespace RecursionTracker.Plugins.VoicepackCombiner.GUI
{
    public partial class DebugForm : Form
    {
        private VoicepackCombiner _voicepackCombiner;
        //private TestDataBinding _test;
        public DebugForm(VoicepackCombiner voicepackCombiner)
        {
            InitializeComponent();

            _voicepackCombiner = voicepackCombiner;
            //TestDataBinding _test = new TestDataBinding();
            ////checkBox1.DataBindings.Add("Checked", _test, "TestDataBindingProperty");
            //checkBox1.DataBindings.Add(new Binding("Checked", _test, "TestDataBindingProperty", true,
            //    DataSourceUpdateMode.OnPropertyChanged));
        }

        private void btnPrintGlobalVoicepack_Click(object sender, EventArgs e)
        {
            //VoicepackCombiner.PrintToConsole(GlobalVariablesPS2.achievementOptions);
            VoicepackExtended globalCopyToPrint = new VoicepackExtended();
            globalCopyToPrint.GetFromGlobal();
            this.txtBoxDebugInfo.AppendText(globalCopyToPrint.ToString());
        }

        private void btnPrintCombinedVoicepack_Click(object sender, EventArgs e)
        {
            this.txtBoxDebugInfo.AppendText(_voicepackCombiner.CombinedVoicepack.ToString());
        }

        private void btnPrintBackupVoicepack_Click(object sender, EventArgs e)
        {
            this.txtBoxDebugInfo.AppendText(_voicepackCombiner.GlobalVoicepackBackup.ToString());
        }

        private void btnClearTxtBox_Click(object sender, EventArgs e)
        {
            this.txtBoxDebugInfo.Clear();
        }
    }
}
