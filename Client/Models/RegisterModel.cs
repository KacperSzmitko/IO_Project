using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    class RegisterModel
    {
        public bool CheckLoginText(string login) {
            if (login.Length >= 3) return true;
            else return false;
        }

        public bool CheckPasswordText(string pass) {
            if (pass.Length >= 8) return true;
            else return false;
        }

        public bool CheckIfPasswordsAreEqual(string pass1, string pass2) {
            if (pass1 == pass2) return true;
            else return false;
        }
    }
}
