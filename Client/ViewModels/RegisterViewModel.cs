using Client.Commands;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private RegisterModel model;

        private string username;
        private string pass1;
        private string pass2;

        private RelayCommand login;

        public bool GoodUsername { get; set; }
        public bool UsernameAvailable { get; set; }
        public bool GoodPass1 { get; set; }
        public bool GoodPass2 { get; set; }

        public string Username {
            get { return username; }
            set {
                if (value != username) {
                    username = value;
                    if (model.CheckUsernameText(username)) GoodUsername = true;
                    else GoodUsername = false;
                    UpdateUsernameBox();
                    if (GoodUsername) _ = UpdateIfUsernameExistAsync();
                }
            }
        }

        public string Pass1 {
            get { return pass1; }
            set {
                if (value != pass1) {
                    pass1 = value;
                    if (model.CheckPasswordText(pass1)) GoodPass1 = true;
                    else GoodPass1 = false;
                    UpdatePass1Box();
                }
            }
        }

        public string Pass2 {
            get { return pass2; }
            set {
                if (value != pass2) {
                    pass2 = value;
                    if (model.CheckPasswordText(pass2) && model.CheckPasswordsAreEqual(Pass1, pass2)) GoodPass2 = true;
                    else GoodPass2 = false;
                    UpdatePass2Box();
                }
            }
        }

        public string UsernameBoxColor {
            get {
                if (String.IsNullOrEmpty(Username)) return "White";
                else if (!GoodUsername || !UsernameAvailable) return "Salmon";
                else return "LightGreen";
            }
        }

        public string Pass1BoxColor {
            get {
                if (String.IsNullOrEmpty(Pass1)) return "White";
                else if (!GoodPass1) return "Salmon";
                else return "LightGreen";
            }
        }

        public string Pass2BoxColor {
            get {
                if (String.IsNullOrEmpty(Pass2)) return "White";
                else if (!GoodPass2) return "Salmon";
                else return "LightGreen";
            }
        }

        public string UsernameInfoVisibility {
            get {
                if (!String.IsNullOrEmpty(Username) && !GoodUsername) return "Visible";
                else return "Collapsed";
            }
        }

        public string UsernameInfoAvailabilityVisibility {
            get {
                if (!String.IsNullOrEmpty(Username) && GoodUsername && !UsernameAvailable) return "Visible";
                else return "Collapsed";
            }
        }

        public string Pass1InfoVisibility {
            get {
                if (!String.IsNullOrEmpty(Pass1) && !GoodPass1) return "Visible";
                else return "Collapsed";
            }
        }

        public string Pass2InfoVisibility {
            get {
                if (!String.IsNullOrEmpty(Pass2) && !GoodPass2) return "Visible";
                else return "Collapsed";
            }
        }
        
        public ICommand LoginCommand {
            get {
                if (login == null) {
                    login = new RelayCommand(_ => {
                        if (model.RegisterUser(Username, Pass2)) navigator.CurrentViewModel = new LoginViewModel(connection, navigator);
                    }, _ => {
                        if (GoodUsername && UsernameAvailable && GoodPass1 && GoodPass2) return true;
                        else return false;
                    });
                }
                return login;
            }
        }
        
        public RegisterViewModel(ServerConnection connection, Navigator navigator) : base(connection, navigator) {
            model = new RegisterModel(this.connection);
            this.GoodUsername = false;
            this.UsernameAvailable = true;
            this.GoodPass1 = false;
            this.GoodPass2 = false;
        }

        private void UpdateUsernameBox() {
            OnPropertyChanged(nameof(Username));
            OnPropertyChanged(nameof(UsernameBoxColor));
            OnPropertyChanged(nameof(UsernameInfoVisibility));
            OnPropertyChanged(nameof(UsernameInfoAvailabilityVisibility));
        }

        private void UpdatePass1Box() {
            OnPropertyChanged(nameof(Pass1BoxColor));
            OnPropertyChanged(nameof(Pass1InfoVisibility));
        }

        private void UpdatePass2Box() {
            OnPropertyChanged(nameof(Pass2BoxColor));
            OnPropertyChanged(nameof(Pass2InfoVisibility));
        }

        private async Task UpdateIfUsernameExistAsync() {
            bool exists = await Task.Run(() => model.CheckUsernameExist(Username));
            if (exists) UsernameAvailable = false;
            else UsernameAvailable = true;
            UpdateUsernameBox();
        }



    }
}
