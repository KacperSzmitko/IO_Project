using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class HomeModel : LoggedModel
    {
        public HomeModel(ServerConnection connection, User user) : base(connection, user) {}

        public bool LogoutUser() {
            int error = ServerCommands.LogoutCommand(ref connection, user.SessionID);
            if (error == (int)ErrorCodes.NO_ERROR) return true;
            else throw new Exception(GetErrorCodeName(error));
        }

    }
}
