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

        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel) {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object parameter) {
            return true;
        }

        public void Execute(object parameter) {
            if (parameter.ToString() == "Login") viewModel.SelectedViewModel = new LoginViewModel();
            else if (parameter.ToString() == "Register") viewModel.SelectedViewModel = new RegisterViewModel();
        }
    }
}
