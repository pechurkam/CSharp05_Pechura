namespace CSharp_Pechura_05.Tools.Navigation
{
    internal enum ViewType
    {
        TableView,
        MainWindow
    }

    interface INavigationModel
    {
        void Navigate(ViewType viewType);
    }
}
