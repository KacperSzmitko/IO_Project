using Client.Commands;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Input;

namespace Client.ViewModels
{
    public class WaitingRoomViewModel : BaseViewModel
    {
        private WaitingRoomModel model;

        private Thread waitingDotsThread;
        private Thread searchGameThread;

        private RelayCommand goHomeCommand;

        private bool successfulSearchStart;
        private bool gameFound;

        private string waitingDots;

        public string Username {
            get { return model.User.Username; }
        }

        public int Elo {
            get { return model.User.Elo; }
        }

        public string WaitingDots {
            get { return waitingDots; }
        }

        public ICommand GoHomeCommand {
            get {
                if (goHomeCommand == null) {
                    goHomeCommand = new RelayCommand(_ => {
                        navigator.CurrentViewModel = new HomeViewModel(connection, navigator, model.User);
                    }, _ => true);
                }
                return goHomeCommand;
            }
        }

        public WaitingRoomViewModel(ServerConnection connection, Navigator navigator, User user) : base(connection, navigator) {
            this.model = new WaitingRoomModel(this.connection, user);
            this.successfulSearchStart = false;
            this.gameFound = false;
            this.waitingDots = ".";
            this.waitingDotsThread = new Thread(UpdateWaitingDotsAsync);
            this.waitingDotsThread.Start();
            this.searchGameThread = new Thread(SearchGameAsync);
            this.searchGameThread.Start();
        }

        private void UpdateWaitingDotsAsync() {
            while (!gameFound) {
                waitingDots = ".";
                OnPropertyChanged(nameof(WaitingDots));
                Thread.Sleep(750);
                waitingDots = "..";
                OnPropertyChanged(nameof(WaitingDots));
                Thread.Sleep(750);
                waitingDots = "...";
                OnPropertyChanged(nameof(WaitingDots));
                Thread.Sleep(750);
            }
        }

        private void SearchGameAsync() {
            successfulSearchStart = model.SearchGame(); 
            if (successfulSearchStart) {
                Opponent opponent = model.GetFoundMatch();
                if (opponent == null) navigator.CurrentViewModel = new HomeViewModel(connection, navigator, model.User);
                navigator.CurrentViewModel = new MatchViewModel(connection, navigator, model.User, opponent);
            } 
            else {
                navigator.CurrentViewModel = new HomeViewModel(connection, navigator, model.User);
            }
        }
    }
}
