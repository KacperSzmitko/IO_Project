using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows.Input;
using Client.Commands;

namespace Client.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public Navigator Navigator { get; set; }
        public ICommand UpdateViewCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseViewModel(Navigator navigator) {
            this.Navigator = navigator;
            this.UpdateViewCommand = new UpdateViewCommand(this.Navigator);
        }

        protected void OnPropertyChanged(string propertyName) {
            if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
