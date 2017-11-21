using Client.Developer.Converter;
using Common;
using Core;
using Developer;
using MongoDB.Bson;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Repository;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Linq;

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

        public DeveloperViewModel(IEventAggregator eventAggregator, IBusyIndicator busyIndicator)
        {
            _busyIndicator = busyIndicator;
            _eventAggregator = eventAggregator;
            _developerRepository = new DeveloperRepository();
            _knowledgeRepository = new KnowledgeRepository();

            _saveCommand = new DelegateCommand(async() => await Save(),  CanExecuteSave);
            _cancelCommand = new DelegateCommand(Cancel);
            _addKnowledgeCommand = new DelegateCommand(OpenKnowledgeEditor);
            _interactionRequest = new InteractionRequest<Confirmation>();

            _developer = new DeveloperModel();

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
                _developer = value;
                NotifyPropertyChanged(nameof(Developer));
                SaveCommand.RaiseCanExecuteChanged();
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

        public string KnowledgeList
        {
            get { return _knowledgeList; }
            set
            {
                _knowledgeList = value;
                NotifyPropertyChanged(nameof(KnowledgeList));
            }
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
            return Developer != null;
        }

        private void SetCurrentModel(DeveloperModel data)
        {
            Developer = data as DeveloperModel;
            KnowledgeConverter converter = new KnowledgeConverter();
            KnowledgeList = converter.ExtractKnowledge(Developer.KnowledgeBase);
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
                KnowledgeList = string.Empty;

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
            KnowledgeList = string.Empty;
        }

        private void AddKnowledge(KnowledgeModel data)
        {
            if(Developer != null)
            {
                Developer.KnowledgeBase.Add(data);
                KnowledgeConverter converter = new KnowledgeConverter();
                KnowledgeList = converter.ExtractKnowledge(Developer.KnowledgeBase);
            }
        }
    }
}
