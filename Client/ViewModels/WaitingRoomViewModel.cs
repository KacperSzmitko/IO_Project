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
        private Thread searchMatchThread;

        private RelayCommand goHomeCommand;

        private bool successfulSearchStart;
        private bool matchFound, stopSearching;

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
                        stopSearching = true;
                        searchMatchThread.Join();
                        if (model.StopSearchingMatch()) navigator.CurrentViewModel = new HomeViewModel(connection, navigator, model.User);
                    }, _ => true);
                }
                return goHomeCommand;
            }
        }

        public WaitingRoomViewModel(ServerConnection connection, Navigator navigator, User user) : base(connection, navigator) {
            this.model = new WaitingRoomModel(this.connection, user);
            this.successfulSearchStart = false;
            this.matchFound = false;
            this.stopSearching = false;
            this.waitingDots = ".";
            this.waitingDotsThread = new Thread(UpdateWaitingDotsAsync);
            this.waitingDotsThread.Start();
            this.searchMatchThread = new Thread(SearchMatchAsync);
            this.searchMatchThread.Start();
        }

        private void UpdateWaitingDotsAsync() {
            while (!matchFound && !stopSearching) {
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

        private void SearchMatchAsync() {
            successfulSearchStart = model.SearchMatch(); 
            if (successfulSearchStart) {
                while (!model.NetworkStreamDataAvailable && !stopSearching) Thread.Sleep(10);
                if (model.NetworkStreamDataAvailable) {
                    Opponent opponent = model.GetFoundMatch();
                    matchFound = true;
                    stopSearching = true;
                    if (opponent == null) navigator.CurrentViewModel = new HomeViewModel(connection, navigator, model.User);
                    navigator.CurrentViewModel = new MatchViewModel(connection, navigator, model.User, opponent);
                }
            } 
            else {
                navigator.CurrentViewModel = new HomeViewModel(connection, navigator, model.User);
            }
        }
    }
}
