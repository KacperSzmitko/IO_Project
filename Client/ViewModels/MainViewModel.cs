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
                return Navigator.CurrentViewModel;
            }
            set {
                Navigator.CurrentViewModel = value;
            }
        }

        public MainViewModel(Navigator navigator) : base(navigator) {
            
            this.CurrentViewModel = new LoginViewModel(navigator);
        }

        public void OnViewChanged(object source, EventArgs args) {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

    }
}
