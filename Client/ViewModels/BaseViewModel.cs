using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Client.Commands;

namespace Client.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        protected ServerConnection connection;
        protected Navigator navigator;
        public ICommand UpdateViewCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel(ServerConnection connection, Navigator navigator) {
            this.connection = connection;
            this.navigator = navigator;
            this.UpdateViewCommand = new UpdateViewCommand(this.connection, this.navigator);
        }

        protected void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
