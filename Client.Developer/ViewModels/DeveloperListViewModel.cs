using Common;
using Core;
using Developer;
using Prism.Commands;
using Prism.Events;
using Repository;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System;
using System.Linq;

namespace Client.Developer.ViewModels
{
    public class DeveloperListViewModel : BasePropertyChanged
    {
        private IDeveloperRepository _developerRepository;
        private ObservableCollection<DeveloperModel> _developers;
        private DelegateCommand<DeveloperModel> _deleteCommand;
        private DelegateCommand<DeveloperModel> _cloneCommand;
        private DelegateCommand _refreshCommand;
        private DelegateCommand _filterCommand;
        private DeveloperModel _selectedItem;
        private readonly IEventAggregator _eventAggregator;
        private readonly IBusyIndicator _busyIndicator;
        private string _filter;
        private DelegateCommand _deleteAllCommand;

        public DeveloperListViewModel(IEventAggregator eventAggregator, IBusyIndicator busyIndicator)
        {
            _eventAggregator = eventAggregator;
            _busyIndicator = busyIndicator;

            _developerRepository = new DeveloperRepository();
            _deleteCommand = new DelegateCommand<DeveloperModel>(async (item) => await DeleteModel(item));
            _refreshCommand = new DelegateCommand(async() => await LoadData());
            _filterCommand = new DelegateCommand(async () => await ExecuteFilter(), CanExecuteFilter);
            _cloneCommand = new DelegateCommand<DeveloperModel>(async(model) => await CloneModel(model));
            _deleteAllCommand = new DelegateCommand(async() => await DeleteAllModel(), CanDeleteAll);

            _eventAggregator.GetEvent<UpdateDeveloperListPubEvent>().Subscribe(async() =>
            {
                await LoadData();
            });
        }

       

        public DelegateCommand<DeveloperModel> DeleteCommand { get { return _deleteCommand; } }

        public DelegateCommand UpdateCommand { get { return _refreshCommand; } }

        public DelegateCommand FilterCommand { get { return _filterCommand; } }

        public DelegateCommand<DeveloperModel> CloneCommand { get { return _cloneCommand; } }

        public DelegateCommand DeleteAllCommand { get { return _deleteAllCommand; } }

        

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
                _eventAggregator.GetEvent<EntityEditPubEvent>().Publish(_selectedItem);
            }
        }


        public string Filter
        {
            get { return _filter; }
            set
            {
                _filter = value;
                NotifyPropertyChanged(nameof(Filter));
                FilterCommand.RaiseCanExecuteChanged();
            }
        }

        private bool CanDeleteAll()
        {
            return Developers != null && Developers.Any();
        }
        private async Task DeleteAllModel()
        {
            var dialogres = Xceed.Wpf.Toolkit.MessageBox.Show("Delete all entries?", "Delete entries", MessageBoxButton.YesNo);
            if (dialogres == System.Windows.MessageBoxResult.Yes)
            {
                _busyIndicator.Busy = true;
                await _developerRepository.DeleteAllAsync();
                await LoadData();
                _busyIndicator.Busy = false;
            }
        }

        private bool CanExecuteFilter()
        {
            return !String.IsNullOrEmpty(Filter) && !String.IsNullOrWhiteSpace(Filter);
        }


        private async Task LoadData()
        {
            _busyIndicator.Busy = true;

            await Task.Delay(2000);
            var developers = await _developerRepository.FindAllAsync();
            Developers = new ObservableCollection<DeveloperModel>(developers);

            DeleteAllCommand.RaiseCanExecuteChanged();

            _busyIndicator.Busy = false;
        }

        private async Task<bool> DeleteModel(DeveloperModel entity)
        {
            var result = false;

            var dialogres = Xceed.Wpf.Toolkit.MessageBox.Show("Delete the selected entity?", "Delete entity", MessageBoxButton.YesNo);
            if (dialogres == System.Windows.MessageBoxResult.Yes)
            {
                _busyIndicator.Busy = true;
                result = await _developerRepository.DeleteAsync(entity);
                if (result)
                {
                    Developers.Remove(entity);
                    _eventAggregator.GetEvent<EntityEditPubEvent>().Publish(new DeveloperModel());
                }

                _busyIndicator.Busy = false;
            }
            DeleteAllCommand.RaiseCanExecuteChanged();
            return result;
        }

        private async Task ExecuteFilter()
        {
            var results = await _developerRepository.FindByTextSearch(Filter);
            Developers = new ObservableCollection<DeveloperModel>(results);
        }

        private async Task CloneModel(DeveloperModel model)
        {
            _busyIndicator.Busy = true;
            
            var clonedModel = await _developerRepository.CloneAsync(model);
            if (clonedModel != null)
            {
                Developers.Add(clonedModel);
            }

            _busyIndicator.Busy = false;
        }

    }
}
