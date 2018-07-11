using System.ComponentModel;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
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
