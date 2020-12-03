using System;
using System.Collections.Generic;
using System.Text;

namespace DbLibrary
{

    public class MatchHistory
    {
        public string id { get; set; }
        public string player1 { get; set; }
        public string player2 { get; set; }
        public string winner { get; set; }
        public string end_time { get; set; }

        public string player1_elo { get; set; }
        public string player2_elo { get; set; }
        public string player1_elo_loss { get; set; }
        public string player2_elo_loss { get; set; }
        public string player1_points { get; set; }
        public string player2_points { get; set; }

    }
}
