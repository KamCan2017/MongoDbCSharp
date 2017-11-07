using System.ComponentModel;

namespace Common
{
    public class BasePropertyChanged : INotifyPropertyChanged
    {
        public BasePropertyChanged()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
