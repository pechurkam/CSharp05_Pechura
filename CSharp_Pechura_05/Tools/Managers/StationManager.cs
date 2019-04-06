using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;


namespace CSharp_Pechura_05.Tools.Managers
{
    internal static class StationManager
    {
        public static event Action StopThreads;

        internal static DataGrid PersonTable { get; set; }

        internal static List<Process> ProcessesList
        {
            get { return _processesList; }
        }

        private static List<Process> _processesList = new List<Process>(Process.GetProcesses());

        internal static void UpdateList()
        {
            _processesList = new List<Process>(Process.GetProcesses());
        }

        internal static void Initialize()
        {
        }

        internal static void CloseApp()
        {
            MessageBox.Show("YOU ARE GOING TO CLOSE THE APP");
            StopThreads?.Invoke();
            Environment.Exit(1);
        }
    }
}

