using Client.Core.Model;
using Common;
using Core;
using Core.QueueClient;
using Developer;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Repository;
using System;
//using WebClient;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json.Linq;
using Core.Interfaces;
using Core.ServiceClient;

namespace Client.Developer.ViewModels
{
    public class DeveloperListViewModel : BasePropertyChanged
    {
        private ObservableCollection<IDeveloper> _developers;
        private readonly DelegateCommand<DeveloperModel> _deleteCommand;
        private readonly DelegateCommand<DeveloperModel> _cloneCommand;
        private readonly DelegateCommand _refreshCommand;
        private readonly DelegateCommand _filterCommand;
        private readonly DelegateCommand _deleteAllCommand;
        private readonly DelegateCommand _simulateCommand;

        private DeveloperModel _selectedItem;
        private readonly IEventAggregator _eventAggregator;
        private readonly IBusyIndicator _busyIndicator;
        private string _filter;

        private QueueClient _userChangedQueueClient;
        private QueueClient _usersSavedQueueClient;

        public DeveloperListViewModel(IEventAggregator eventAggregator, IBusyIndicator busyIndicator)
        {
            _eventAggregator = eventAggregator;
            _busyIndicator = busyIndicator;

            _deleteCommand = new DelegateCommand<DeveloperModel>(async (item) => await DeleteModel(item));
            _refreshCommand = new DelegateCommand(async() => await LoadData());
            _filterCommand = new DelegateCommand(async () => await ExecuteFilter(), CanExecuteFilter);
            _cloneCommand = new DelegateCommand<DeveloperModel>(async(model) => await CloneModel(model));
            _deleteAllCommand = new DelegateCommand(async() => await DeleteAllModel(), CanDeleteAll);
            _simulateCommand = new DelegateCommand(async () => await Simulate());
            _eventAggregator.GetEvent<UpdateDeveloperListPubEvent>().Subscribe(async() =>
            {
                await LoadData();
            });

            //Queue to reacte when a user is saved or changed
            async void userChangedCallback(string payload, string severity) => await UserChangedCallback(payload, severity);
            _userChangedQueueClient = new QueueClient(QueueConfig.ExchangeUser, QueueConfig.SeverityUser, userChangedCallback);


            //Queue to reacte when multiple users are saved
            void usersSavedCallback(string payload, string severity) => UsersSavedCallback(payload, severity);
            _usersSavedQueueClient = new QueueClient(QueueConfig.ExchangeUser, QueueConfig.SeverityMultipleUsers, usersSavedCallback);
        }

       

        public DelegateCommand<DeveloperModel> DeleteCommand { get { return _deleteCommand; } }

        public DelegateCommand UpdateCommand { get { return _refreshCommand; } }

        public DelegateCommand FilterCommand { get { return _filterCommand; } }

        public DelegateCommand<DeveloperModel> CloneCommand { get { return _cloneCommand; } }

        public DelegateCommand DeleteAllCommand { get { return _deleteAllCommand; } }

        public DelegateCommand SimulateCommand { get { return _simulateCommand; } }

        public ObservableCollection<IDeveloper> Developers
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
                await ServiceClient<IDeveloperService>.ExecuteAsync(o => o.DeleteAllAsync());
                Developers = new ObservableCollection<IDeveloper>();

                DeleteAllCommand.RaiseCanExecuteChanged();

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

            await Task.Delay(500);

            //var developerSvc = new DeveloperService();
            //var items = await developerSvc.FindAllAsync();

            var developers = await ServiceClient<IDeveloperService>.ExecuteAsync(o => o.FindAllAsync());
            Developers = new ObservableCollection<IDeveloper>(developers);

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
                result = await ServiceClient<IDeveloperService>.ExecuteAsync(o => o.DeleteAsync(entity));
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
            var results = await ServiceClient<IDeveloperService>.ExecuteAsync(o => o.FindByTextSearchAsync(Filter));
            Developers = new ObservableCollection<IDeveloper>(results);
        }

        private async Task CloneModel(DeveloperModel model)
        {
            _busyIndicator.Busy = true;
            
            var clonedModel = await ServiceClient<IDeveloperService>.ExecuteAsync(o => o.CloneAsync(model));
            if (clonedModel != null)
            {
                Developers.Add(clonedModel);
            }

            _busyIndicator.Busy = false;
        }


        private async Task Simulate()
        {
            await Task.Factory.StartNew(async () => 
            {
                _busyIndicator.Busy = true;
                _busyIndicator.Message = "100 thousand users are generated...";
                //one million data
                int max = 100000;
                var users = new List<DeveloperModel>();
                for (int i = 1; i <= max; i++)
                {
                    var user = new DeveloperModel()
                    {
                        Name = "user_" + i,
                        CompanyName = "suse",
                       Gender = Gender.Male
                    };

                    users.Add(user);
                }

                //Save all users
                await ServiceClient<IDeveloperService>.ExecuteAsync(o => o.SaveEntitiesAsync(users));               

                _busyIndicator.Busy = false;
            });            
        }


        private async Task UserChangedCallback(string payload, string severity)
        {
            var model = JsonConvert.DeserializeObject<DeveloperModel>(payload);
            if (model == null)
                return;
            _busyIndicator.Busy = true;

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var user = Developers.FirstOrDefault(d => d.ID == model.ID);
                if (user == null)
                    Developers.Add(model);
                else
                {
                    var index = Developers.IndexOf(user);
                    Developers.Remove(user);
                    Developers.Insert(index, model);
                }

            }, System.Windows.Threading.DispatcherPriority.Background);
            _busyIndicator.Busy = false;

        }

        private void UsersSavedCallback(string payload, string severity)
        {
            var models = JsonConvert.DeserializeObject<IEnumerable<DeveloperModel>>(payload);
            if (models == null)
                return;

            _busyIndicator.Busy = true;
            var list = Developers.ToList();
            list.AddRange(models);
            Developers = new ObservableCollection<IDeveloper>(list);
            DeleteAllCommand.RaiseCanExecuteChanged();
            _busyIndicator.Busy = false;
        }
    }
    
}
