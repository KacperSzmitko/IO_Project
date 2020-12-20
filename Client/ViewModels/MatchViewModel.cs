using Client.Commands;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace Client.ViewModels
{
    class MatchViewModel : BaseViewModel
    {
        private MatchModel model;

        private RelayCommand setCellCommand;

        private Thread sendUserMoveThread, getOpponentMoveThread, pauseBetweenRoundsThread;

        private MoveResult moveResult;

        private bool pauseBetweenRounds, matchEnded;

        public CellStatus[] CellsStatus {
            get { return model.CellsStatus; }
        }

        public string UserTurnInfo {
            get {
                if (moveResult == MoveResult.USER_WON) return "Wygrałeś rundę";
                else if (moveResult == MoveResult.USER_LOST) return "Przegrałeś rundę";
                else if (moveResult == MoveResult.DRAW) return "Remis";
                else if (model.UserTurn) return "Twój ruch";
                else return "Ruch przeciwnika";
            }                
        }

        public int UserScore {
            get { return model.UserScore; }
        }

        public int OpponentScore {
            get { return model.OpponentScore; }
        }

        public string UserName {
            get { return model.User.Username; }
        }

        public int UserElo {
            get { return model.User.Elo; }
        }

        public string OpponentName {
            get { return model.Opponent.Username; }
        }

        public int OpponentElo {
            get { return model.Opponent.Elo; }
        }


        public ICommand SetCellCommand {
            get {
                if (setCellCommand == null) {
                    setCellCommand = new RelayCommand(cellIndex => {
                        int ci = Int32.Parse((string)cellIndex);
                        model.UserTurn = false;
                        sendUserMoveThread = new Thread(() => SendUserMoveAsync(ci));
                        sendUserMoveThread.Start();
                    }, cellIndex => {
                        int ci = Int32.Parse((string)cellIndex);
                        if (model.CellsStatus[ci] == CellStatus.EMPTY && model.UserTurn && !pauseBetweenRounds) return true;
                        else return false;
                    });
                }
                return setCellCommand;
            }
        }

        public MatchViewModel(ServerConnection connection, Navigator navigator, User user, Opponent opponent) : base(connection, navigator) {
            this.model = new MatchModel(connection, user, opponent);
            this.pauseBetweenRounds = false;
            this.matchEnded = false;
            if (opponent.StartsMatch) {
                getOpponentMoveThread = new Thread(GetOpponentMoveAsync);
                getOpponentMoveThread.Start();
            }
        }

        private void SendUserMoveAsync(int ci) {
            moveResult = model.SendUserMove(ci);
            OnPropertyChanged(nameof(CellsStatus));
            OnPropertyChanged(nameof(UserScore));
            OnPropertyChanged(nameof(OpponentScore));
            OnPropertyChanged(nameof(UserTurnInfo));
            if (moveResult == MoveResult.USER_WON || moveResult == MoveResult.USER_LOST || moveResult == MoveResult.DRAW) {
                pauseBetweenRoundsThread = new Thread(PauseBetweenRoundsAsync);
                pauseBetweenRoundsThread.Start();
            }
            if (pauseBetweenRoundsThread != null) pauseBetweenRoundsThread.Join();
            if (!matchEnded) {
                getOpponentMoveThread = new Thread(GetOpponentMoveAsync);
                getOpponentMoveThread.Start();
            }
        }

        private void GetOpponentMoveAsync() {
            moveResult = model.GetOpponentMove();
            OnPropertyChanged(nameof(CellsStatus));
            OnPropertyChanged(nameof(UserScore));
            OnPropertyChanged(nameof(OpponentScore));
            OnPropertyChanged(nameof(UserTurnInfo));
            if (moveResult == MoveResult.USER_WON || moveResult == MoveResult.USER_LOST || moveResult == MoveResult.DRAW) {
                pauseBetweenRoundsThread = new Thread(PauseBetweenRoundsAsync);
                pauseBetweenRoundsThread.Start();
            }
        }

        private void PauseBetweenRoundsAsync() {
            pauseBetweenRounds = true;

            //If game ended, get endGameInfo from server and go to EndGameViewModel
            if (model.UserScore == model.ScoreToWin || model.OpponentScore == model.ScoreToWin) {
                matchEnded = true;
                EndGameInfo endGameInfo = model.GetEndGameInfo();
                Thread.Sleep(2500);
                navigator.CurrentViewModel = new EndGameViewModel(connection, navigator, model.User, model.Opponent, endGameInfo);
            }
            else {
                Thread.Sleep(2500);
                model.EmptyCells();
                moveResult = MoveResult.ROUND_NOT_OVER;
                pauseBetweenRounds = false;
                OnPropertyChanged(nameof(CellsStatus));
                OnPropertyChanged(nameof(UserTurnInfo));
            }
        }
    }
}
