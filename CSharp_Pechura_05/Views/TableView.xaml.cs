using System.Windows.Controls;
using CSharp_Pechura_05.Tools.Navigation;
using CSharp_Pechura_05.ViewModels;

namespace CSharp_Pechura_05.Views
{
    /// <summary>
    /// Interaction logic for TableView.xaml
    /// </summary>
    public partial class TableView : UserControl, INavigatable
    {
        public TableView()
        {
            InitializeComponent();
            DataContext = new TableViewModel();
        }
    }
}
