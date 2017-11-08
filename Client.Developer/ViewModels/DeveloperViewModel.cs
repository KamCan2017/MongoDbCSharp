﻿using Client.Developer.Converter;
using Common;
using Developer;
using MongoDB.Bson;
using Prism.Commands;
using Prism.Events;
using Repository;
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
        private string _knowledgeList;

        public DeveloperViewModel(IEventAggregator eventAggregator)
        {
            _developerRepository = new DeveloperRepository();
            _saveCommand = new DelegateCommand(async() => await Save(),  CanExecuteSave);
            _cancelCommand = new DelegateCommand(Cancel);

            _developer = new DeveloperModel();

            eventAggregator.GetEvent<EntityEditPubEvent>().Subscribe(data => SetCurrentModel(data));
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

       
        
        private async Task<bool> Save()
        {
            if (!Developer.IsValid)
                return false;

            var parameters = _knowledgeList.Split(',');
            if (parameters != null && parameters.Length > 0)
            {
                if (Developer.KnowledgeBase == null)
                    Developer.KnowledgeBase = new System.Collections.Generic.List<KnowledgeModel>();

                Developer.KnowledgeBase.Clear();

                foreach (string str in parameters)
                {
                    if (!string.IsNullOrEmpty(str))
                        Developer.KnowledgeBase.Add(new KnowledgeModel() { Technology = str });
                }
            }

            if (Developer.ID == ObjectId.Empty)
                await _developerRepository.SaveAsync(Developer);
            else
                await _developerRepository.UpdateAsync(Developer);

            Developer = new DeveloperModel();
            KnowledgeList = string.Empty;
            return true;
        }

        private void Cancel()
        {
            Developer = new DeveloperModel();
            KnowledgeList = string.Empty;
        }
    }
}
