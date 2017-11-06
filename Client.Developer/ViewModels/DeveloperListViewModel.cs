using Developer;
using Microsoft.Practices.Prism.Commands;
using Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Developer
{
    public class DeveloperListViewModel : INotifyPropertyChanged
    {
        private IDeveloperRepository _developerRepository;
        private ObservableCollection<DeveloperModel> _developers;
        private DelegateCommand<DeveloperModel> _deleteCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _filterCommand;
        private DeveloperModel _selectedItem;

        public DeveloperListViewModel()
        {
            _developerRepository = new DeveloperRepository();
            _deleteCommand = new DelegateCommand<DeveloperModel>(async (item) => await DeleteModel(item));
            _refreshCommand = new DelegateCommand(() => LoadData());
            _filterCommand = new DelegateCommand(async () => await ExecuteFilter());
        }

       
        public DelegateCommand<DeveloperModel> DeleteCommand { get { return _deleteCommand; } }

        public DelegateCommand UpdateCommand { get { return _refreshCommand; } }

        public DelegateCommand FilterCommand { get { return _filterCommand; } }

        public ObservableCollection<DeveloperModel> Developers
        {
            get { return _developers; }
            set
            {
                _developers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Developers)));
            }
        }

        public DeveloperModel SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                _selectedItem = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
                DataExchanger.FireData(_selectedItem, null);
            }
        }


        public string Filter { get; set; }      

        public event PropertyChangedEventHandler PropertyChanged;

       
        public async void LoadData()
        {
            var developers = await _developerRepository.FindAllAsync();
            Developers = new ObservableCollection<DeveloperModel>(developers);
        }

        private async Task<bool> DeleteModel(DeveloperModel entity)
        {
            var result = false;

            var dialogres = Xceed.Wpf.Toolkit.MessageBox.Show("Delete the selected entity?", "Delete entity", MessageBoxButton.YesNo);
            if (dialogres == System.Windows.MessageBoxResult.Yes)
            {
                result = await _developerRepository.DeletedAsync(entity);
                if (result)
                {
                    Developers.Remove(entity);
                }
            }
            return result;
        }

        private async Task ExecuteFilter()
        {
            var results = await _developerRepository.FindByTextSearch(Filter);
            Developers = new ObservableCollection<DeveloperModel>(results);
        }       
    }
}
