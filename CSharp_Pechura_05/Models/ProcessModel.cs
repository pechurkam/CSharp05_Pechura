using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.VisualBasic.Devices;
namespace CSharp_Pechura_05.Models
{
    class ProcessModel : INotifyPropertyChanged
    {
        private double _processMemoryPercent = -1;
        private Process _process;
        private bool _isSelected;

        #region Properties

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        private double ProcessMemoryPercent
        {
            get
            {
                return (_processMemoryPercent < 0)
                    ? (_processMemoryPercent = 100.0 / (new ComputerInfo()).TotalPhysicalMemory)
                    : _processMemoryPercent;
            }
        }

        public string Name { get; set; }
        public string Title { get; set; }
        public long Id { get; set; }

        public string FileLocation
        {
            get
            {
                try
                {
                    return _process.MainModule.FileName;
                }
                catch (Exception e)
                {
                    return "";
                }
            }
        }


        public long MemoryWorkingSet { get; set; }

        public Process ProcessObj
        {
            set { _process = value; }
        }

        public string MemoryUsagePercent
        {
            get { return (MemoryWorkingSet * ProcessMemoryPercent).ToString("0.00"); }
        }

        public string MemoryUsageMB
        {
            get { return (MemoryWorkingSet / (1024.0 * 1024.0)).ToString("0.00"); }
        }

        public string CPU
        {
            get
            {
                return "0";
              
            }
        }

        public ProcessModuleCollection Modules
        {
            get
            {
                try
                {
                    ProcessModuleCollection a = _process.Modules;
                    return _process.Modules;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
        public ProcessThreadCollection Threads
        {
            get
            {

                try
                {
                    return _process.Threads;
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }
        #endregion

        public void DeleteProcess()
        {
            _process.Kill();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}