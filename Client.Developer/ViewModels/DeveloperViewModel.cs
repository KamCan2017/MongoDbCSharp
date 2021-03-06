﻿using Common;
using Core;
using Core.Interfaces;
using Core.ServiceClient;
using Developer;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Repository;
using System.Collections.Generic;
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
        private readonly IBusyIndicator _busyIndicator;
        private readonly IEventAggregator _eventAggregator;
        private DelegateCommand _addKnowledgeCommand;
        private InteractionRequest<Confirmation> _interactionRequest;
        private DelegateCommand<KnowledgeModel> _removeKnowledgeCommand;
        private int _selectedIndex;

        public DeveloperViewModel(IEventAggregator eventAggregator, IBusyIndicator busyIndicator,
            IKnowledgeRepository knowledgeRepository)
        {
            _busyIndicator = busyIndicator;
            _eventAggregator = eventAggregator;

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

        public List<string> Genders
        {
            get
            {
                return new List<string>() { Gender.Female, Gender.Male };
            }
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
            if (data != null)
                Developer = data;
            else
                Developer = new DeveloperModel();
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
                if (Developer.KnowledgeBase != null)
                {
                    var knowledgeToSave = Developer.KnowledgeBase.Where(p => string.IsNullOrEmpty(p.ID));
                    if (knowledgeToSave.Any())
                    {
                        foreach (var entity in knowledgeToSave)
                        {
                            var savedObj = await ServiceClient<IKnowledgeService>.ExecuteAsync(o => o.SaveAsync(entity));
                        }
                    }
                }

                if (string.IsNullOrEmpty(Developer.ID))
                    await ServiceClient<IDeveloperService>.ExecuteAsync(o => o.SaveAsync(Developer));
                else
                    await ServiceClient<IDeveloperService>.ExecuteAsync(o => o.UpdateAsync(Developer));

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
                //if(result)
                //    //Publish event to update the developer list
                //    _eventAggregator.GetEvent<UpdateDeveloperListPubEvent>().Publish();
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

                if(Developer.KnowledgeBase.Any(p => p.ID == data.ID)
                    || Developer.KnowledgeBase.Any(p => p.Language == data.Language && p.Technology == data.Technology))
                {
                    return;
                }
                Developer.KnowledgeBase.Add(data);
                SeletedIndex = 0;
            }
        }

        private void RemoveKnowledge(KnowledgeModel item)
        {
            Developer.KnowledgeBase.Remove(item);
        }

    }
}
