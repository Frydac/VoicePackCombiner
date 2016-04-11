using System;
using System.Windows.Forms;
using RecursionTracker.Plugins.VoicepackCombiner.Voicepack;

namespace RecursionTracker.Plugins.VoicepackCombiner.GUI
{
    public partial class DebugForm : Form
    {
        private VoicepackCombiner _voicepackCombiner;
        public DebugForm(VoicepackCombiner voicepackCombiner)
        {
            InitializeComponent();

            _voicepackCombiner = voicepackCombiner;
        }

        private void btnPrintGlobalVoicepack_Click(object sender, EventArgs e)
        {
            var globalCopyToPrint = new VoicepackExtended();
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
