using System.ComponentModel;
using System.Windows;

namespace CSharp_Pechura_05.Tools
{
    internal interface ILoaderOwner : INotifyPropertyChanged
    {
        Visibility LoaderVisibility { get; set; }
        bool IsControlEnabled { get; set; }
    }
}
