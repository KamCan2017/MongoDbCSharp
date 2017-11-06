using System.ComponentModel;

namespace Client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
        }   
            

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
