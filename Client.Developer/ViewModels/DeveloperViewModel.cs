using Client.Developer.Converter;
using Common;
using Core;
using Developer;
using MongoDB.Bson;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Repository;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.Developer.ViewModels
{
    public class DeveloperViewModel: BasePropertyChanged
        {
        private DeveloperModel _developer;
        private DelegateCommand _saveCommand;
        private ICommand _cancelCommand;

        private IDeveloperRepository _developerRepository;
        private IKnowledgeRepository _knowledgeRepository;

        private readonly IBusyIndicator _busyIndicator;
        private readonly IEventAggregator _eventAggregator;
        private string _knowledgeList;
        private DelegateCommand _addKnowledgeCommand;
        private InteractionRequest<Confirmation> _interactionRequest;
        private DelegateCommand<KnowledgeModel> _removeKnowledgeCommand;
        private int _selectedIndex;

        public DeveloperViewModel(IEventAggregator eventAggregator, IBusyIndicator busyIndicator)
        {
            _busyIndicator = busyIndicator;
            _eventAggregator = eventAggregator;
            _developerRepository = new DeveloperRepository();
            _knowledgeRepository = new KnowledgeRepository();

            _saveCommand = new DelegateCommand(async() => await Save(),  CanExecuteSave);
            _cancelCommand = new DelegateCommand(Cancel);
            _addKnowledgeCommand = new DelegateCommand(OpenKnowledgeEditor, CanOpenKnowledgeEditor);
            _removeKnowledgeCommand = new DelegateCommand<KnowledgeModel>(item => RemoveKnowledge(item));

            _interactionRequest = new InteractionRequest<Confirmation>();

            Developer = new DeveloperModel();
            SeletedIndex = 0;

            _eventAggregator.GetEvent<EntityEditPubEvent>().Subscribe(data => SetCurrentModel(data));
            _eventAggregator.GetEvent<AddKnowledgePubEvent>().Subscribe(data => AddKnowledge(data));
        }


        public IInteractionRequest InteractionRequest
        {
            get { return _interactionRequest; }
        }

        public DeveloperModel Developer
        {
            get { return _developer; }
            set
            {
                if (_developer != null)
                    _developer.PropertyChanged -= _developer_PropertyChanged;

                _developer = value;  
                if(_developer != null)
                    _developer.PropertyChanged += _developer_PropertyChanged;

                NotifyPropertyChanged(nameof(Developer));
                SaveCommand.RaiseCanExecuteChanged();
                AddKnowledgeCommand.RaiseCanExecuteChanged();
            }
        }


        public DelegateCommand AddKnowledgeCommand
        {
            get { return _addKnowledgeCommand; }
        }

        public DelegateCommand SaveCommand
        {
            get { return _saveCommand; }
        }

        public ICommand CancelCommand
        {
            get { return _cancelCommand; }
        }

        public DelegateCommand<KnowledgeModel> RemoveKnowledgeCommand
        {
            get { return _removeKnowledgeCommand; }
        }      
        
        public int SeletedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged(nameof(SeletedIndex));
            }
        }

        private void _developer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
            AddKnowledgeCommand.RaiseCanExecuteChanged();
        }

        private void OpenKnowledgeEditor()
        {
            _interactionRequest.Raise(
            new Confirmation(),
            OnWindowClosed);
        }

        private void OnWindowClosed(Confirmation confirmation)
        {
            if (confirmation.Confirmed)
            {
                //perform the confirmed action...
            }
            else
            {

            }
        }
        private bool CanExecuteSave()
        {
            return Developer != null && Developer.IsValid;
        }

        private bool CanOpenKnowledgeEditor()
        {
            return Developer != null && Developer.IsValid;
        }

        private void SetCurrentModel(DeveloperModel data)
        {
            Developer = data as DeveloperModel;
            KnowledgeConverter converter = new KnowledgeConverter();
        }

        private async Task<bool> Save()
        {
            bool result = false;
            try
            {
                _busyIndicator.Busy = true;

                if (!Developer.IsValid)
                    return false;

                //Save the new knowledge
                var knowledgeToSave = Developer.KnowledgeBase.Where(p => p.ID == ObjectId.Empty);
                if (knowledgeToSave.Any())
                {
                    foreach(var entity in knowledgeToSave)
                    {
                        var savedObj = await _knowledgeRepository.SaveAsync(entity);
                        Developer.KnowledgeIds.Add(savedObj.ID);
                    }
                }

                if (Developer.ID == ObjectId.Empty)
                    await _developerRepository.SaveAsync(Developer);
                else
                    await _developerRepository.UpdateAsync(Developer);

                await Task.Delay(2000);

                Developer = new DeveloperModel();

                result = true;
                return result;

            }
            catch (System.Exception)
            {
                throw;
            }

            finally
            {
                _busyIndicator.Busy = false;
                if(result)
                    //Publish event to update the developer list
                    _eventAggregator.GetEvent<UpdateDeveloperListPubEvent>().Publish();
            }

        }

        private void Cancel()
        {
            Developer = new DeveloperModel();

            //Publish event to update the developer list
            _eventAggregator.GetEvent<UpdateDeveloperListPubEvent>().Publish();
        }

        private void AddKnowledge(KnowledgeModel data)
        {
            if(Developer != null)
            {
                if (Developer.KnowledgeBase == null)
                    Developer.KnowledgeBase = new ObservableCollection<KnowledgeModel>();

                Developer.KnowledgeBase.Add(data);
                SeletedIndex = 0;
            }
        }

        private void RemoveKnowledge(KnowledgeModel item)
        {
            Developer.KnowledgeBase.Remove(item);
            if (item.ID != ObjectId.Empty)
                Developer.KnowledgeIds.Remove(item.ID);
        }

    }
}
