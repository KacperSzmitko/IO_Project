using Client.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Client.ViewModels
{
    class MainViewModel : BaseViewModel
    {
        public BaseViewModel CurrentViewModel {
            get {
                return navigator.CurrentViewModel;
            }
            set {
                navigator.CurrentViewModel = value;
            }
        }

        public MainViewModel(ServerConnection connection, Navigator navigator) : base(connection, navigator) {
            
            this.CurrentViewModel = new LoginViewModel(this.connection, this.navigator);
        }

        public void OnViewChanged(object source, EventArgs args) {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }
}
