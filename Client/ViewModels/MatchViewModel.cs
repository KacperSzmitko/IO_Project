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

        private Thread sendUserMoveThread, getOpponentMoveThread;

        private MoveResult moveResult;

        public CellStatus[] CellsStatus {
            get { return model.CellsStatus; }
        }

        public string UserTurnInfo {
            get {
                if (moveResult == MoveResult.USER_WON) return "Wygrałeś rundę";
                else if (moveResult == MoveResult.USER_LOST) return "Przegrałeś rundę";
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
                        if (model.CellsStatus[ci] == CellStatus.EMPTY && model.UserTurn) return true;
                        else return false;
                    });
                }
                return setCellCommand;
            }
        }

        public MatchViewModel(ServerConnection connection, Navigator navigator, User user, Opponent opponent) : base(connection, navigator) {
            this.model = new MatchModel(connection, user, opponent);
            if (opponent.StartsMatch) {
                getOpponentMoveThread = new Thread(GetOpponentMoveAsync);
                getOpponentMoveThread.Start();
            }
        }

        private void SendUserMoveAsync(int ci) {
            moveResult = model.SendUserMove(ci);
            UpdateProperties();
            if (moveResult == MoveResult.USER_WON || moveResult == MoveResult.USER_LOST) model.EmptyCells();
            getOpponentMoveThread = new Thread(GetOpponentMoveAsync);
            getOpponentMoveThread.Start();
        }

        private void GetOpponentMoveAsync() {
            moveResult = model.GetOpponentMove();
            UpdateProperties();
            if (moveResult == MoveResult.USER_WON || moveResult == MoveResult.USER_LOST) model.EmptyCells();
        }

        private void UpdateProperties() {
            OnPropertyChanged(nameof(CellsStatus));
            OnPropertyChanged(nameof(UserTurnInfo));
            OnPropertyChanged(nameof(UserScore));
            OnPropertyChanged(nameof(OpponentScore));
        }
    }
}
