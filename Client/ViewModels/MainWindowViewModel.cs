using Client.Developer.Views;
using System.ComponentModel;
using System.Windows.Controls;

namespace Client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private DeveloperView _viewDeveloperEditor;
        private DeveloperList _viewDeveloperList;
        private UserControl _contentView;

        public MainWindowViewModel()
        {
            _viewDeveloperEditor = new DeveloperView();
            _viewDeveloperList = new DeveloperList();

            ContentView = _viewDeveloperList;
        }

        public object SelectedItem { get; set; }

        public UserControl ContentView
        {
            get { return _contentView; }
            set
            {
                _contentView = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContentView)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
