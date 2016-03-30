using System;
using System.Diagnostics;
using RecursionTracker.Plugins.VoicePackCombiner.GUI;
#if DEBUG
using System.IO;
using System.Runtime.InteropServices;
#endif

namespace RecursionTracker.Plugins.VoicePackCombiner
{
    /// <summary>
    /// This class is used by the host/main program to use the plugin.
    /// It also serves as a manager class, i.e owner of the main model and view classes
    /// When the main form loads, it inserts a menuitem to be able to start the VoicePackCombinerForm
    /// </summary>
    public class VoicePackCombinerPlugin : PluginBase
    {
        readonly public VoicePackCombiner VoicePackCombiner = new VoicePackCombiner();
        public VoicePackCombinerForm VoicePackCombinerForm;
        private VoicePackCombinerMenuItems _voicePackCombinerMenuItems;

#if DEBUG
        private readonly DebugHelpers _debugHelpers;
#endif

        public VoicePackCombinerPlugin()
        {
            Debug.WriteLine("VoicePackCombinerPlugin Construction");

            m_pluginInformation = new PluginInformation(
                "Voice Pack Combiner",
                "Load multiple voice packs simultaniously",
                "Emile Vrijdags (aka Frydac)",
                null);
#if DEBUG
            _debugHelpers = new DebugHelpers(VoicePackCombiner);
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

            CreateVoicePackCombinerMenuItemsInMainGui();
#if DEBUG
            _debugHelpers.ShowDebugForm();
#endif
        }

        private void CreateVoicePackCombinerMenuItemsInMainGui()
        {
            string parentMenuItemName = "modsToolStripMenuItem";
            _voicePackCombinerMenuItems = new VoicePackCombinerMenuItems(parentMenuItemName,
                VoicePackCombiner, VoicePackCombinerForm, m_core.GetMainForm());
            Debug.WriteLine("After inserting voicepackcombiner menu items");
        }
    }

#if DEBUG
    /// <summary>
    /// Contains a debugForm and shows console window to help while debugging
    /// </summary>
    internal class DebugHelpers
    {
        private DebugForm _debugForm;

        /// <summary>
        /// Import win32 function to show console window
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        /// <summary>
        /// Import win32 function to dispose console window
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern int FreeConsole();

        private VoicePackCombiner _voicePackCombiner;

        internal DebugHelpers(VoicePackCombiner voicePackCombiner)
        {
            _voicePackCombiner = voicePackCombiner;
            ShowCmdConsoleWindow();
            SetDebugOutputToConsole();
        }

        private static void ShowCmdConsoleWindow()
        {
            AllocConsole();
        }

        /// <summary>
        /// Calls to Debug.Write* will be printed in the console window
        /// </summary>
        private void SetDebugOutputToConsole()
        {
            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            Debug.Listeners.Add(new ConsoleTraceListener());
        }

        internal void ShowDebugForm()
        {
            if(_debugForm==null || !_debugForm.Visible)
            { 
                _debugForm = new DebugForm(_voicePackCombiner);
                _debugForm.Show();
            }
        }

        ~DebugHelpers()
        {
            FreeConsole();
        }
    }
#endif

}
