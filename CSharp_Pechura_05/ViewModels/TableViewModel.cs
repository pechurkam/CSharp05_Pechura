using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CSharp_Pechura_05.Models;
using CSharp_Pechura_05.Tools;
using CSharp_Pechura_05.Tools.Managers;


namespace CSharp_Pechura_05.ViewModels
{
    internal class TableViewModel : BaseViewModel
    {
        public TableViewModel()
        {
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            StartWorkingThread();
            StationManager.StopThreads += StopWorkingThread;

        }

        #region Fields

        private List<System.Diagnostics.Process> _processList = StationManager.ProcessesList;
        private ProcessModel _selectedProcess;
        private string[] _sortCases = { "Id", "Name", "Window Title", "Memory Usage" };
        private int _sortCase = 0;
        private int _selectedIndex = -1;

        private RelayCommand<object> _openFileCommand;
        private RelayCommand<object> _deleteCommand;

        private Thread _workingThread;

        private CancellationToken _token;
        private CancellationTokenSource _tokenSource;

        #endregion

        #region Properties

        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                if (value != -1)
                {
                    _selectedIndex = value;
                }
            }
        }

    
        public IEnumerable<string> SortCasesEnum
        {
            get { return _sortCases; }
        }

        public int Sort
        {
            get { return _sortCase; }
            set
            {
                _sortCase = value;
                OnPropertyChanged("ProcessList");
            }
        }

        public ProcessModel SelectedProcess
        {
            get { return _selectedProcess; }
            set
            {

                _selectedProcess = value;

                OnPropertyChanged();
                OnPropertyChanged("ProcessModules");
                OnPropertyChanged("ProcessThreads");

            }
        }

        public int SelectedSortDescriptionByIndex { get; set; }

        public IEnumerable<Models.ProcessModel> ProcessList
        {
            get
            {

                _processList = new List<System.Diagnostics.Process>(System.Diagnostics.Process.GetProcesses());

                IEnumerable<Models.ProcessModel> l = (from row in _processList
                                                     
                                                 select new Models.ProcessModel
                                              {
                                                  MemoryWorkingSet = row.WorkingSet64,
                                                  Id = row.Id,
                                                  Name = row.ProcessName,
                                                  Title = row.MainWindowTitle,
                                                  UserName = row.StartInfo.UserName,
                                                  IsResponding = row.Responding,
                                                  ProcessObj = row
                                              });

                switch (Sort)
                {
                    case 0:
                        l =   l.OrderBy(o => o.Id) ;
                        break;
                    case 1:
                        l =   l.OrderBy(o => o.Name) ;
                        break;
                    case 2:
                        l =  l.OrderBy(o => o.Title) ;
                        break;
                    case 3:
                        l = l.OrderBy(o => o.MemoryWorkingSet) ;
                        break;
                }

                return l;
            }
        }

        public RelayCommand<object> OpenFileCommand
        {
            get { return _openFileCommand ?? (_openFileCommand = new RelayCommand<object>(OpenFileImplementation, CanDoWithProcess)); }
        }

        public RelayCommand<object> DeleteCommand
        {
            get
            {
                return _deleteCommand ?? (_deleteCommand = new RelayCommand<object>(
                           DeleteImplementation, CanDoWithProcess));
            }
        }

        #endregion

        #region Process Properties

        public ProcessModuleCollection ProcessModules
        {
            get { return SelectedProcess?.Modules; }
        }

        public ProcessThreadCollection ProcessThreads
        {
            get { return SelectedProcess?.Threads; }
        }

        #endregion

        private async void OpenFileImplementation(object obj)
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run((Action)(() =>
            {
                if (string.IsNullOrWhiteSpace((string)SelectedProcess.FileLocation))
                {
                    MessageBox.Show("Access denied to this process", "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                base.OnPropertyChanged("ProcessList");
                string location = "/select, \"" + SelectedProcess.FileLocation + "\"";
                System.Diagnostics.Process.Start("explorer.exe", location);
            }));
            LoaderManeger.Instance.HideLoader();
        }

        private async void DeleteImplementation(object obj)
        {
            LoaderManeger.Instance.ShowLoader();
            await Task.Run(() => {
                try
                {
                    SelectedProcess?.DeleteProcess();
                    OnPropertyChanged("ProcessList");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Access denied to this process", "ERROR",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
            LoaderManeger.Instance.HideLoader();

        }

        private bool CanDoWithProcess(object obj)
        {
            return SelectedProcess != null;
        }

        private void StartWorkingThread()
        {
            _workingThread = new Thread(WorkingThreadProcess);
            _workingThread.Start();
        }


        private void WorkingThreadProcess()
        {
            int i = 0;
            while (!_token.IsCancellationRequested)
            {
                ProcessModel selProc = SelectedProcess;

                OnPropertyChanged("ProcessList");
                SelectedProcess = selProc;
                
                for (int j = 0; j < 10; j++)
                {
                    Thread.Sleep(500);
                 
                    if (_token.IsCancellationRequested)
                        break;
                }
                
                i++;
                
            }
            
        }

        internal void StopWorkingThread()
        {
            _tokenSource.Cancel();
            _workingThread.Join(2000);
            _workingThread.Abort();
            _workingThread = null;
        }
    }
}
