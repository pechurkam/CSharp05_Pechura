using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using CSharp_Pechura_05.Tools.Managers;
using CSharp_Pechura_05.Tools.Navigation;
using CSharp_Pechura_05.ViewModels;

namespace CSharp_Pechura_05.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window, IContentOwner
    {
        public ContentControl ContentControl
        {
            get { return _contentControl; }
        }


        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
            
            NavigationManager.Instance.Initialize(new InitializationNavigationModel(this));
            NavigationManager.Instance.Navigate(ViewType.TableView);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            StationManager.CloseApp();
        }
    }

}
