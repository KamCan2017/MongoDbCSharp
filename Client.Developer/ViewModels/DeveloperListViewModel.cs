using Developer;
using Microsoft.Practices.Prism.Commands;
using Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Client.Developer
{
    public class DeveloperListViewModel : INotifyPropertyChanged
    {
        private IDeveloperRepository _developerRepository;
        private ObservableCollection<DeveloperModel> _developers;
        private DelegateCommand<DeveloperModel> _deleteCommand;

        public DeveloperListViewModel()
        {
            _developerRepository = new DeveloperRepository();
            _deleteCommand = new DelegateCommand<DeveloperModel>(async (item) => await DeleteModel(item));
        }

       
        public DelegateCommand<DeveloperModel> DeleteCommand { get { return _deleteCommand; } }

        public ObservableCollection<DeveloperModel> Developers
        {
            get { return _developers; }
            set
            {
                _developers = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Developers)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public async void LoadData()
        {
           var developers = await _developerRepository.FindAllAsync();
            Developers = new ObservableCollection<DeveloperModel>(developers);
        }

        private async Task<bool> DeleteModel(DeveloperModel entity)
        {
            var result = false;
            result = await _developerRepository.DeletedAsync(entity);
            if (result)
            {
                Developers.Remove(entity);
            }
            return result;
        }
    }
}
