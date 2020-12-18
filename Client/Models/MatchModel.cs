using Shared;
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

        public MatchModel(ServerConnection connection, User user, Opponent opponent) : base(connection, user) {
            this.opponent = opponent;
            this.userStartsRound = !opponent.StartsMatch; //"o" always starts
            this.userTurn = !opponent.StartsMatch;
            this.userScore = 0;
            this.opponentScore = 0;
            this.cellsStatus = EmptyCells();
        }

        public bool SendUserMove(int move) {
            ServerCommands.SendUserMoveResponse response = ServerCommands.SendUserMoveCommand(ref connection, user.SessionID, move);
            if (response.error == (int)ErrorCodes.NO_ERROR) {
                userTurn = false;
                userScore = response.userScore;
                opponentScore = response.opponentScore;
                if (userStartsRound) CellsStatus[move] = CellStatus.USER_O;
                else CellsStatus[move] = CellStatus.USER_X;
                return true;
            }
            else throw new Exception(GetErrorCodeName(response.error));
        }

        public bool GetOpponentMove() {
            ServerCommands.GetOpponentMoveResponse response = ServerCommands.GetOpponentMoveCommand_responseOnly(ref connection);
            if (response.error == (int)ErrorCodes.NO_ERROR) {
                userTurn = true;
                userScore = response.userScore;
                opponentScore = response.opponentScore;
                if (userStartsRound) CellsStatus[response.opponentMove] = CellStatus.OPPONENT_X;
                else CellsStatus[response.opponentMove] = CellStatus.OPPONENT_O;
                return true;
            }
            else throw new Exception(GetErrorCodeName(response.error));
        }

        private CellStatus[] EmptyCells() {
            return new CellStatus[9] { CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY };
        }
    }
}
