using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class MatchModel : LoggedModel
    {
        private readonly Opponent opponent;
        private Player userPlayer, opponentPlayer;

        public MatchModel(ServerConnection connection, User user, Opponent opponent) : base(connection, user) {
            this.opponent = opponent;
            this.userPlayer = new Player();
            this.opponentPlayer = new Player();
        }
    }
}
