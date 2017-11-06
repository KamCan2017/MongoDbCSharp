using Client.Developer.Views;
using Microsoft.Practices.Prism.Commands;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System;
using Client.Developer;
using Client.Developer.ViewModels;

namespace Client.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private IBaseViewModel _selectedViewModel;
        private Visibility _visible;
        private DelegateCommand<object> _showViewCommand;

        public MainWindowViewModel()
        {
            _showViewCommand = new DelegateCommand<object>(item => ShowView(item));
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

        public DelegateCommand<object> ShowViewCommand
        {
            get { return _showViewCommand; }
        }       

        public event PropertyChangedEventHandler PropertyChanged;

        private void ShowView(object item)
        {
            if (item is IBaseViewModel)
            {
                if(_selectedViewModel != null)
                {
                    _selectedViewModel.Visible = Visibility.Collapsed;
                }
                _selectedViewModel = item as IBaseViewModel;
                _selectedViewModel.Visible = Visibility.Visible;
            }
        }
    }
}
