using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        private RegisterModel model;

        private string login;
        private string pass1;
        private string pass2;

        public bool GoodLogin { get; set; }
        public bool GoodPass1 { get; set; }
        public bool GoodPass2 { get; set; }

        public string Login {
            get { return login; }
            set {
                if (value != login) {
                    login = value;
                    if (model.CheckLoginText(login)) GoodLogin = true;
                    else GoodLogin = false;
                    OnPropertyChanged(nameof(Login));
                    OnPropertyChanged(nameof(LoginBoxColor));
                    OnPropertyChanged(nameof(LoginInfoVisibility));
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
                    OnPropertyChanged(nameof(Pass1BoxColor));
                    OnPropertyChanged(nameof(Pass1InfoVisibility));
                }
            }
        }

        public string Pass2 {
            get { return pass2; }
            set {
                if (value != pass2) {
                    pass2 = value;
                    if (model.CheckPasswordText(pass2) && model.CheckIfPasswordsAreEqual(Pass1, pass2)) GoodPass2 = true;
                    else GoodPass2 = false;
                    OnPropertyChanged(nameof(Pass2BoxColor));
                    OnPropertyChanged(nameof(Pass2InfoVisibility));
                }
            }
        }

        public string LoginBoxColor {
            get {
                if (String.IsNullOrEmpty(Login)) return "White";
                else if (!GoodLogin) return "Salmon";
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

        public string LoginInfoVisibility {
            get {
                if (!String.IsNullOrEmpty(Login) && !GoodLogin) return "Visible";
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



        public RegisterViewModel() {
            model = new RegisterModel();
        }



    }
}
