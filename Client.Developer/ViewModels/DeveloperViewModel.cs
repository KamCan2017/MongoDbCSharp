using Client.Developer.Converter;
using Developer;
using Microsoft.Practices.Prism.Commands;
using MongoDB.Bson;
using Repository;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.Developer
{
    public class DeveloperViewModel: INotifyPropertyChanged
        {
        private DeveloperModel _developer;
        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        private IDeveloperRepository _developerRepository;
        private string _knowledgeList;

        public DeveloperViewModel()
        {
            _developerRepository = new DeveloperRepository();
            _saveCommand = new DelegateCommand(async() => await Save());
            _cancelCommand = new DelegateCommand(Cancel);

            _developer = new DeveloperModel();
            DataExchanger.FireData += GetData;
        }

        private void GetData(object data, EventArgs e)
        {
            if (data is DeveloperModel)
            {
                DeveloperModel = data as DeveloperModel;
                KnowledgeConverter converter = new KnowledgeConverter();
                KnowledgeList = converter.ExtractKnowledge(DeveloperModel.KnowledgeBase);
            }
        }

        public DeveloperModel DeveloperModel
        {
            get { return _developer; }
            set
            {
                _developer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeveloperModel)));
            }
        }

        public ICommand SaveCommand
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KnowledgeList)));
            }
        }

       
        public event PropertyChangedEventHandler PropertyChanged;


        
        private async Task<bool> Save()
        {
            if (_developer != null && _developer.IsValid)
            {
                var parameters = _knowledgeList.Split(',');
                if (parameters != null && parameters.Length > 0)
                {
                    if (DeveloperModel.KnowledgeBase == null)
                        DeveloperModel.KnowledgeBase = new System.Collections.Generic.List<KnowledgeModel>();

                    DeveloperModel.KnowledgeBase.Clear();

                    foreach (string str in parameters)
                    {
                        if(!string.IsNullOrEmpty(str))
                          DeveloperModel.KnowledgeBase.Add(new KnowledgeModel() { Technology = str });
                    }
                }

                if (DeveloperModel.ID == ObjectId.Empty)
                    await _developerRepository.SaveAsync(DeveloperModel);
                else
                    await _developerRepository.UpdateAsync(DeveloperModel);

                DeveloperModel = new DeveloperModel();
                KnowledgeList = string.Empty;
                return true;
            }

            return false;
        }

        private void Cancel()
        {
            DeveloperModel = new DeveloperModel();
            KnowledgeList = string.Empty;
        }
    }
}
