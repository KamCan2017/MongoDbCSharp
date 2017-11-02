using Developer;
using Repository;
using System.Collections.Generic;
using System.ComponentModel;

namespace Client.Developer
{
    public class DeveloperListViewModel : INotifyPropertyChanged
    {
        private IDeveloperRepository _developerRepository;
        private IEnumerable<DeveloperModel> _developers;

        public DeveloperListViewModel()
        {
            _developerRepository = new DeveloperRepository();
        }


        public IEnumerable<DeveloperModel> Developers
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
           Developers = await _developerRepository.FindAllAsync();
        }     
    }
}
