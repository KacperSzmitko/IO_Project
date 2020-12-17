using Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.ViewModels
{
    class MatchViewModel : BaseViewModel
    {
        private MatchModel model;

        public MatchViewModel(ServerConnection connection, Navigator navigator, User user, Opponent opponent) : base(connection, navigator) {
            this.model = new MatchModel(connection, user, opponent);
        }
    }
}
