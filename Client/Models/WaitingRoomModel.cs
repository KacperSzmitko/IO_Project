using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class WaitingRoomModel : LoggedModel
    {
        public WaitingRoomModel(ServerConnection connection, User user) : base(connection, user) { }

        public bool SearchGame() {
            int error = ServerCommands.SearchMatchCommand(ref connection, user.SessionID);
            if (error == (int)ErrorCodes.NO_ERROR) return true;
            else throw new Exception(GetErrorCodeName(error));
        }

        public Opponent GetFoundMatch() {
            ServerCommands.GetFoundMatchCommandResponse response = ServerCommands.GetFoundMatchCommand_responseOnly(ref connection);
            if (response.error != (int)ErrorCodes.NO_ERROR) throw new Exception(GetErrorCodeName(response.error));
            return new Opponent(response.opponentName, response.opponentRank, !Convert.ToBoolean(response.isCross));
        }


    }
}
