using Common;
using Core;
using Developer;
using Prism.Commands;
using Prism.Events;
using Repository;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Developer.ViewModels
{
    public class DeveloperListViewModel : BasePropertyChanged
    {
        private IDeveloperRepository _developerRepository;
        private ObservableCollection<DeveloperModel> _developers;
        private DelegateCommand<DeveloperModel> _deleteCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _filterCommand;
        private DeveloperModel _selectedItem;
        private readonly IEventAggregator _eventAggregator;
        private readonly IBusyIndicator _busyIndicator;


        public DeveloperListViewModel(IEventAggregator eventAggregator, IBusyIndicator busyIndicator)
        {
            _eventAggregator = eventAggregator;
            _busyIndicator = busyIndicator;

            _developerRepository = new DeveloperRepository();
            _deleteCommand = new DelegateCommand<DeveloperModel>(async (item) => await DeleteModel(item));
            _refreshCommand = new DelegateCommand(async() => await LoadData());
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
                NotifyPropertyChanged(nameof(Developers));
            }
        }

        public DeveloperModel SelectedItem
        {
            get { return _selectedItem; }

            set
            {
                _selectedItem = value;
                NotifyPropertyChanged(nameof(SelectedItem));
                if(_selectedItem != null)
                  _eventAggregator.GetEvent<EntityEditPubEvent>().Publish(_selectedItem);
            }
        }


        public string Filter { get; set; }      


       
        private async Task LoadData()
        {
            _busyIndicator.Busy = true;

            await Task.Delay(2000);
            var developers = await _developerRepository.FindAllAsync();
            Developers = new ObservableCollection<DeveloperModel>(developers);

            _busyIndicator.Busy = false;
        }

        private async Task<bool> DeleteModel(DeveloperModel entity)
        {
            var result = false;

            var dialogres = Xceed.Wpf.Toolkit.MessageBox.Show("Delete the selected entity?", "Delete entity", MessageBoxButton.YesNo);
            if (dialogres == System.Windows.MessageBoxResult.Yes)
            {
                _busyIndicator.Busy = true;
                result = await _developerRepository.DeletedAsync(entity);
                await Task.Delay(2000);
                if (result)
                {
                    Developers.Remove(entity);
                    _eventAggregator.GetEvent<EntityEditPubEvent>().Publish(new DeveloperModel());
                }

                _busyIndicator.Busy = false;
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
