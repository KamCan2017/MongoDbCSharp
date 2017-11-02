using Client.Core.Model;
using Developer;
using Microsoft.Practices.Prism.Commands;
using Repository;
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

        public DeveloperViewModel()
        {
            _developerRepository = new DeveloperRepository();
            _saveCommand = new DelegateCommand(async() => await Save());
            _cancelCommand = new DelegateCommand(Cancel);

            _developer = new DeveloperModel();
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

        public string KnowledgeList { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private async Task<bool> Save()
        {
            if (_developer != null && _developer.IsValid)
            {
                if(!string.IsNullOrEmpty(KnowledgeList))
                {
                    var parameters = KnowledgeList.Split(',');
                    if(parameters != null && parameters.Length > 0)
                    {
                        foreach(string str in parameters)
                        {
                            _developer.KnowledgeBase.Add(new KnowledgeModel() { Technology = str });
                        }
                    }
                }

                await _developerRepository.SaveAsync(_developer);
                return true;
            }

            return false;
        }

        private void Cancel()
        { }
    }
}
