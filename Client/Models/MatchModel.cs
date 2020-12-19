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
        private int old_userScore, old_opponentScore;
        private int userScore, opponentScore;
        private int movesCounter, roundCounter;
        private readonly int scoreToWin;

        public Opponent Opponent {
            get { return opponent; }
        }

        public CellStatus[] CellsStatus {
            get { return cellsStatus; }
        }

        public bool UserTurn {
            get { return userTurn; }
            set { userTurn = value; }
        }

        public int UserScore {
            get { return userScore; }
        }

        public int OpponentScore {
            get { return opponentScore; }
        }

        public int ScoreToWin {
            get { return scoreToWin; }
        }

        public MatchModel(ServerConnection connection, User user, Opponent opponent) : base(connection, user) {
            this.opponent = opponent;
            this.userStartsRound = !opponent.StartsMatch; //"x" always starts
            this.userTurn = !opponent.StartsMatch;
            this.userScore = 0;
            this.opponentScore = 0;
            this.old_userScore = 0;
            this.old_opponentScore = 0;
            this.roundCounter = 0;
            this.movesCounter = 0;
            this.scoreToWin = Variables.scoreToWin;
            this.cellsStatus = new CellStatus[9];
            EmptyCells();
        }

        public MoveResult SendUserMove(int move) {
            ServerCommands.SendUserMoveCommandResponse response = ServerCommands.SendUserMoveCommand(ref connection, user.SessionID, move);
            if (response.error == (int)ErrorCodes.NO_ERROR) {
                userTurn = false;
                userScore = response.userScore;
                opponentScore = response.opponentScore;
                movesCounter++;

                if (userStartsRound) CellsStatus[move] = CellStatus.USER_X;
                else CellsStatus[move] = CellStatus.USER_O;

                if (userScore > old_userScore) {
                    movesCounter = 0;
                    old_userScore = userScore;
                    roundCounter++;
                    userStartsRound = !userStartsRound;
                    return MoveResult.USER_WON;
                }
                else if (opponentScore > old_opponentScore) {
                    movesCounter = 0;
                    old_opponentScore = opponentScore;
                    roundCounter++;
                    userStartsRound = !userStartsRound;
                    return MoveResult.USER_LOST;
                }
                else if (movesCounter == 9) {
                    movesCounter = 0;
                    roundCounter++;
                    userStartsRound = !userStartsRound;
                    return MoveResult.DRAW;
                }
                else return MoveResult.ROUND_NOT_OVER;
            }
            else throw new Exception(GetErrorCodeName(response.error));
        }

        public MoveResult GetOpponentMove() {
            ServerCommands.GetOpponentMoveCommandResponse response = ServerCommands.GetOpponentMoveCommand_responseOnly(ref connection);
            if (response.error == (int)ErrorCodes.NO_ERROR) {
                userTurn = true;
                userScore = response.userScore;
                opponentScore = response.opponentScore;
                movesCounter++;

                if (userStartsRound) CellsStatus[response.opponentMove] = CellStatus.OPPONENT_O;
                else CellsStatus[response.opponentMove] = CellStatus.OPPONENT_X;

                if (userScore > old_userScore) {
                    movesCounter = 0;
                    old_userScore = userScore;
                    roundCounter++;
                    userStartsRound = !userStartsRound;
                    return MoveResult.USER_WON;
                }
                else if (opponentScore > old_opponentScore) {
                    movesCounter = 0;
                    old_opponentScore = opponentScore;
                    roundCounter++;
                    userStartsRound = !userStartsRound;
                    return MoveResult.USER_LOST;
                }
                else if (movesCounter == 9) {
                    movesCounter = 0;
                    roundCounter++;
                    userStartsRound = !userStartsRound;
                    return MoveResult.DRAW;
                }
                else return MoveResult.ROUND_NOT_OVER;
            }
            else throw new Exception(GetErrorCodeName(response.error));
        }

        public void EmptyCells() {
            cellsStatus = new CellStatus[9] { CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY, CellStatus.EMPTY };
        }

        public EndGameInfo GetEndGameInfo() {
            ServerCommands.GetEndGameInfoCommandResposne response = ServerCommands.GetEndGameInfoCommand_responseOnly(ref connection);
            if (response.error == (int)ErrorCodes.NO_ERROR) {
                bool userWon;
                int userEloDiffrence = response.newUserElo - user.Elo;
                int oppnentEloDiffrence = response.newOpponentElo - opponent.Elo;
                if (userEloDiffrence > 0) userWon = true;
                else userWon = false;
                return new EndGameInfo(userWon, user.Elo, response.newUserElo, userEloDiffrence, opponent.Elo, response.newOpponentElo, oppnentEloDiffrence);
            }
            else throw new Exception(GetErrorCodeName(response.error));
        }
    }
}
