using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using RecursionTracker.Plugins.VoicepackCombiner.GUI;

namespace RecursionTracker.Plugins.VoicepackCombiner.DebugTools
{
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

        private VoicepackCombiner _voicepackCombiner;

        internal DebugHelpers(VoicepackCombiner VoicepackCombiner)
        {
            _voicepackCombiner = VoicepackCombiner;
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
            System.Diagnostics.Debug.Listeners.Add(new ConsoleTraceListener());
        }

        internal void ShowDebugForm()
        {
            if(_debugForm==null || !_debugForm.Visible)
            { 
                _debugForm = new DebugForm(_voicepackCombiner);
                _debugForm.Show();
            }
        }

        ~DebugHelpers()
        {
            FreeConsole();
        }
    }
}