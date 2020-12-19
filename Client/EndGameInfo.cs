using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class EndGameInfo
    {
        private readonly bool userWon;

        private readonly int oldUserElo;
        private readonly int newUserElo;
        private readonly int userEloDiffrence;

        private readonly int oldOpponentElo;
        private readonly int newOpponentElo;
        private readonly int opponentEloDiffrence;

        public bool UserWon {
            get { return userWon; }
        }

        public int OldUserElo {
            get { return oldUserElo; }
        }

        public int NewUserElo {
            get { return newUserElo; }
        }

        public int UserEloDiffrence {
            get { return userEloDiffrence; }
        }

        public int OldOpponentElo {
            get { return oldOpponentElo; }
        }

        public int NewOpponentElo {
            get { return newOpponentElo; }
        }

        public int OpponentEloDiffrence {
            get { return opponentEloDiffrence; }
        }

        public EndGameInfo(bool userWon, int oldUserElo, int newUserElo, int userEloDiffrence, int oldOpponentElo, int newOpponentElo, int opponentEloDiffrence) {
            this.userWon = userWon;
            this.oldUserElo = oldUserElo;
            this.newUserElo = newUserElo;
            this.userEloDiffrence = userEloDiffrence;
            this.oldOpponentElo = oldOpponentElo;
            this.newOpponentElo = newOpponentElo;
            this.opponentEloDiffrence = opponentEloDiffrence;
        }
    }
}
