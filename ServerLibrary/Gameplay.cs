using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLibrary
{
    public class Gameplay
    {
        public Player p1{get; set;}
        public Player p2{get; set;}
        public string[] board { get; set; }
        public int p1Points { get; set; }
        public int p2Points { get; set; }
        public int pointsToWin { get; set; }

        public Gameplay(Player p1,Player p2,int boardSize,int pointsToWin)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.board = new string[boardSize];
            this.pointsToWin = pointsToWin;
            p1Points = 0;
            p2Points = 0;
        }

        ////// PUT GAME LOGIC HERE
    }
}
