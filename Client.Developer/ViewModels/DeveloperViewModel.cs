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
        private DelegateCommand _saveCommand;
        private ICommand _cancelCommand;

        private IDeveloperRepository _developerRepository;
        private string _knowledgeList;

        public DeveloperViewModel()
        {
            _developerRepository = new DeveloperRepository();
            _saveCommand = new DelegateCommand(async() => await Save(),  CanExecuteSave);
            _cancelCommand = new DelegateCommand(Cancel);

            _developer = new DeveloperModel();
            DataExchanger.FireData += GetData;
        }

        private bool CanExecuteSave()
        {
            return Developer != null;
        }

        private void GetData(object data, EventArgs e)
        {
            if (data is DeveloperModel)
            {
                Developer = data as DeveloperModel;
                KnowledgeConverter converter = new KnowledgeConverter();
                KnowledgeList = converter.ExtractKnowledge(Developer.KnowledgeBase);
            }
        }

        public DeveloperModel Developer
        {
            get { return _developer; }
            set
            {
                _developer = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Developer)));
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(KnowledgeList)));
            }
        }

       
        public event PropertyChangedEventHandler PropertyChanged;


        
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
