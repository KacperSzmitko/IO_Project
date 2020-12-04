using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class RankingModel : LoggedModel
    {
        private readonly string rankingXML;

        public string RankingXML {
            get { return rankingXML; }
        }

        public RankingModel(ServerConnection connection, User user) : base(connection, user) {
            this.rankingXML = GetRankingXML();
        }

        private string GetRankingXML() {
            ServerCommands.GetRankingCommandResponse response = ServerCommands.GetRankingCommand(ref connection, user.SessionID);
            if (response.error != (int)ErrorCodes.NO_ERROR) throw new Exception(GetErrorCodeName(response.error));
            return response.rankingXML;
        }
    }
}
