using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class MatchHistoryModel : LoggedModel
    {
        private readonly string matchHistoryXML;

        public string MatchHistoryXML {
            get { return matchHistoryXML; }
        }

        public MatchHistoryModel(ServerConnection connection, User user) : base(connection, user) {
            this.matchHistoryXML = GetMatchHistoryXML();
        }

        private string GetMatchHistoryXML() {
            ServerCommands.GetMatchHistoryCommandResponse response = ServerCommands.GetMatchHistoryCommand(ref connection, user.SessionID);
            if (response.error != (int)ErrorCodes.NO_ERROR) throw new Exception(GetErrorCodeName(response.error));
            return response.matchHistoryXML;
        }

    }
}
