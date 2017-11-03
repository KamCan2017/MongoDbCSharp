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
        private ListBoxItem _selectedItem;

        public MainWindowViewModel()
        {
            _viewDeveloperEditor = new DeveloperView();
            _viewDeveloperList = new DeveloperList();

            ContentView = _viewDeveloperList;
        }

        public ListBoxItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                if(_selectedItem != null)
                {
                    if ((string)_selectedItem.Content == "Developer List")
                        ContentView = _viewDeveloperList;
                    else
                        ContentView = _viewDeveloperEditor;
                }
            }
        }

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
