using Client.Commands;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Client.ViewModels
{
    class EndGameViewModel : BaseViewModel
    {
        private EndGameModel model;
        private RelayCommand goHomeCommand;

        private readonly EndGameInfo endGameInfo;

        public string GameResult {
            get {
                if (endGameInfo.UserWon) return "Zwycięstwo!";
                else return "Porażka!";
            }
        }

        public string UserName {
            get { return model.User.Username; }
        }

        public string OpponentName {
            get { return model.Opponent.Username; }
        }

        public int OldUserElo {
            get { return endGameInfo.OldUserElo; }
        }

        public int NewUserElo {
            get { return endGameInfo.NewUserElo; }
        }

        public string UserEloDiffrence {
            get {
                if (endGameInfo.UserWon) return "+" + endGameInfo.UserEloDiffrence.ToString();
                else return "-" + (endGameInfo.UserEloDiffrence * -1).ToString(); 
            }
        }

        public string UserEloDiffrenceColor {
            get {
                if (endGameInfo.UserWon) return "Green";
                else return "Red";
            }
        }

        public int OldOpponentElo {
            get { return endGameInfo.OldOpponentElo; }
        }

        public int NewOpponentElo {
            get { return endGameInfo.NewOpponentElo; }
        }

        public string OpponentEloDiffrence {
            get {
                if (endGameInfo.UserWon) return "-" + (endGameInfo.OpponentEloDiffrence * -1).ToString();
                else return "+" + endGameInfo.OpponentEloDiffrence.ToString();
            }
        }

        public string OpponentEloDiffrenceColor {
            get {
                if (endGameInfo.UserWon) return "Red";
                else return "Green";
            }
        }

        public EndGameViewModel(ServerConnection connection, Navigator navigator, User user, Opponent opponent, EndGameInfo endGameInfo) : base(connection, navigator) {
            this.model = new EndGameModel(connection, user, opponent);
            this.endGameInfo = endGameInfo;
        }

        public ICommand GoHomeCommand {
            get {
                if (goHomeCommand == null) {
                    goHomeCommand = new RelayCommand(_ => {
                        model.User.Elo = endGameInfo.NewUserElo;
                        navigator.CurrentViewModel = new HomeViewModel(connection, navigator, model.User);
                    }, _ => true);
                }
                return goHomeCommand;
            }
        }
    }
}
