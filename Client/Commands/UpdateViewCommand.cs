using Client.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Client.Commands
{
    class UpdateViewCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private ServerConnection connection;
        private Navigator navigator;

        public UpdateViewCommand(ServerConnection connection, Navigator navigator) {
            this.connection = connection;
            this.navigator = navigator;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            if (parameter.ToString() == "Login") navigator.CurrentViewModel = new LoginViewModel(connection, navigator);
            else if (parameter.ToString() == "Register") navigator.CurrentViewModel = new RegisterViewModel(connection, navigator);
        }
    }
}
