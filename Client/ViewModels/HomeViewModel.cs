using Client.Commands;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private HomeModel model;
        private RelayCommand logoutCommand;

        public string Username {
            get { return model.User.Username; }
        }

        public int Elo {
            get { return model.User.Elo; }
        }

        public ICommand LogoutCommand {
            get {
                if (logoutCommand == null) {
                    logoutCommand = new RelayCommand(_ => {
                        if (model.LogoutUser()) navigator.CurrentViewModel = new LoginViewModel(connection, navigator);
                    }, _ => true);
                }
                return logoutCommand;
            }
        }

        public HomeViewModel(ServerConnection connection, Navigator navigator, User user) : base(connection, navigator) {
            this.model = new HomeModel(this.connection, user);
        }
    }
}
