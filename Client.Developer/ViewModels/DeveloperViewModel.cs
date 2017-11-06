using Client.Developer.ViewModels;
using Developer;
using Microsoft.Practices.Prism.Commands;
using Repository;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.Developer
{
    public class DeveloperViewModel: INotifyPropertyChanged, IBaseViewModel
    {
        private DeveloperModel _developer;
        private ICommand _saveCommand;
        private ICommand _cancelCommand;

        private IDeveloperRepository _developerRepository;
        private string _knowledgeList;
        private Visibility _visible;

        public DeveloperViewModel()
        {
            _developerRepository = new DeveloperRepository();
            _saveCommand = new DelegateCommand(async() => await Save());
            _cancelCommand = new DelegateCommand(Cancel);

            _developer = new DeveloperModel();
            Visible = Visibility.Collapsed;
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


        public Visibility Visible
        {
            get { return _visible; }
            set
            {
                _visible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Visible)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        
        private async Task<bool> Save()
        {
            if (_developer != null && _developer.IsValid)
            {
                if(!string.IsNullOrEmpty(_knowledgeList))
                {
                    var parameters = _knowledgeList.Split(',');
                    if(parameters != null && parameters.Length > 0)
                    {
                        foreach(string str in parameters)
                        {
                            DeveloperModel.KnowledgeBase.Add(new KnowledgeModel() { Technology = str });
                        }
                    }
                }

                await _developerRepository.SaveAsync(DeveloperModel);
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
            Visible = Visibility.Collapsed;
        }
    }
}
