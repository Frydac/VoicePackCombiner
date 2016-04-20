using System;
using System.Diagnostics;
using RecursionTracker.Plugins.VoicepackCombiner.DebugTools;
using RecursionTracker.Plugins.VoicepackCombiner.GUI;

#if DEBUG

#endif

namespace RecursionTracker.Plugins.VoicepackCombiner
{
    /// <summary>
    /// This class is used by the host/main program to use the plugin.
    /// It also serves as a manager class, i.e owner of the main model and view classes
    /// When the main form loads, it inserts a menuitem to be able to start the VoicepackCombinerForm
    /// </summary>
    public class VoicepackCombinerPlugin : PluginBase
    {
        readonly public VoicepackCombiner VoicepackCombiner = new VoicepackCombiner();
        public VoicepackCombinerForm VoicepackCombinerForm;
        private VoicepackCombinerMenuItems _voicepackCombinerMenuItems;

#if DEBUG
        private readonly DebugHelpers _debugHelpers;
#endif

        public VoicepackCombinerPlugin()
        {
            Debug.WriteLine("VoicepackCombinerPlugin Constructor Called");

            m_pluginInformation = new PluginInformation(
                "Voicepack Combiner",
                "Load multiple voicepacks simultaniously",
                "Emile Vrijdags (aka Frydac)",
                null);
#if DEBUG
            _debugHelpers = new DebugHelpers(VoicepackCombiner);
#endif
        }

        public override Guid GetGuid()
        {
            return new Guid("DE638D66-15A5-44A2-8E38-ED20CDC716BB");
        }

        public override void OnMainFormLoad()
        {
            base.OnMainFormLoad();
            Debug.WriteLine("OnMainFormLoad called");

            CreateVoicepackCombinerMenuItemsInMainGui();
#if DEBUG
            _debugHelpers.ShowDebugForm();
#endif
        }

        private void CreateVoicepackCombinerMenuItemsInMainGui()
        {
            string parentMenuItemName = "modsToolStripMenuItem";
            _voicepackCombinerMenuItems = new VoicepackCombinerMenuItems(parentMenuItemName,
                VoicepackCombiner, VoicepackCombinerForm, m_core.GetMainForm());
            Debug.WriteLine("After inserting voicepackcombiner menu items");
        }
    }

#if DEBUG
#endif

}
