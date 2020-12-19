using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    class EndGameModel : LoggedModel
    {
        private readonly Opponent opponent;

        public Opponent Opponent {
            get { return opponent; }
        }
        public EndGameModel(ServerConnection connection, User user, Opponent opponent) : base(connection, user) {
            this.opponent = opponent;
        }
    }
}
