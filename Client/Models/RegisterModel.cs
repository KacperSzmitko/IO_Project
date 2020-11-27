using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Client.Models
{
    class RegisterModel : BaseModel
    {
        public bool CheckUsernameText(string username) {
            if (username.Length >= 3) return true;
            else return false;
        }

        public bool CheckUsernameExist(string username) {
            int result = ServerCommands.CheckUsernameExistCommand(ref connection, username);
            if (result == 0) return false;
            else return true;
        }

        public bool CheckPasswordText(string pass) {
            if (pass.Length >= 8) return true;
            else return false;
        }

        public bool CheckPasswordsAreEqual(string pass1, string pass2) {
            if (pass1 == pass2) return true;
            else return false;
        }

        public RegisterModel(ServerConnection connection) : base(connection) {
            
        }
    }
}
