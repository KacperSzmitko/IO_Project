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

        public CellStatus[] CellsStatus {
            get { return model.CellsStatus; }
        }

        public string UserTurnInfo {
            get {
                if (model.UserTurn) return "Twój ruch";
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
                        sendUserMoveThread = new Thread(() => SendUserMoveAsync(ci));
                    }, cellIndex => {
                        int ci = Int32.Parse((string)cellIndex);
                        if (model.CellsStatus[ci] == CellStatus.EMPTY && model.UserTurn) return true;
                        else return false;
                    });
                }
                return setCellCommand;
            }
        }

        



        public MatchViewModel(ServerConnection connection, Navigator navigator, User user, Opponent opponent, bool userStartsRound) : base(connection, navigator) {
            this.model = new MatchModel(connection, user, opponent, userStartsRound);
            if (!userStartsRound) getOpponentMoveThread = new Thread(GetOpponentMoveAsync);
        }

        private void SendUserMoveAsync(int ci) {
            if (model.SendUserMove(ci)) {
                UpdateProperties();
                getOpponentMoveThread = new Thread(GetOpponentMoveAsync);
            }
        }

        private void GetOpponentMoveAsync() {
            if (model.GetOpponentMove()) {
                UpdateProperties();
            }
        }

        private void UpdateProperties() {
            OnPropertyChanged(nameof(CellsStatus));
            OnPropertyChanged(nameof(UserTurnInfo));
            OnPropertyChanged(nameof(UserScore));
            OnPropertyChanged(nameof(OpponentScore));
        }
    }
}
