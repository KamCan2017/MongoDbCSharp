﻿using Common;
using Core;

namespace Client.ViewModels
{
    public class MainWindowViewModel : BasePropertyChanged, IBusyIndicator
    {
        private bool _isBusy;
        private string _message;

        public MainWindowViewModel()
        {
            Message = "Please Wait...";
        }

        public bool Busy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                if(!_isBusy)
                {
                    Message = "Please Wait...";
                }
                NotifyPropertyChanged(nameof(Busy));
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyPropertyChanged(nameof(Message));
            }
        }
    }
   
}
