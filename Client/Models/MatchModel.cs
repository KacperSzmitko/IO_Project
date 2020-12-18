using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class MatchModel : LoggedModel
    {
        private readonly Opponent opponent;

        private CellStatus[] cellsStatus;
        private bool userTurn, userStartsRound;
        private int userScore, opponentScore;

        public Opponent Opponent {
            get { return opponent; }
        }

        public CellStatus[] CellsStatus {
            get { return cellsStatus; }
        }

        public bool UserTurn {
            get { return userTurn; }
        }

        public int UserScore {
            get { return userScore; }
        }

        public int OpponentScore {
            get { return opponentScore; }
        }

        public MatchModel(ServerConnection connection, User user, Opponent opponent, bool userStartsRound) : base(connection, user) {
            this.opponent = opponent;
            this.userStartsRound = userStartsRound;
            this.userTurn = userStartsRound;
            this.userScore = 0;
            this.opponentScore = 0;
            this.cellsStatus = EmptyCells();
        }

        public bool SendUserMove(int ci) {
            throw new NotImplementedException();
        }

        public bool GetOpponentMove() {
            throw new NotImplementedException();
        }

        private CellStatus[] EmptyCells() {
            return new CellStatus[9] { CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY };
        }
    }
}
