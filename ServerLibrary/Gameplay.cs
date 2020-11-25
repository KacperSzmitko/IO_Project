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

		public  List<int> p1Positions = new List<int>(); //{ get; set; }
		public  List<int> p2Positions = new List<int>();//{ get; set; }

		public Gameplay(Player p1,Player p2,int boardSize,int pointsToWin)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.board = new string[boardSize];
            this.pointsToWin = pointsToWin;
            p1Points = 0;
            p2Points = 0;
        }
		// zapisanie danego ruchu i sprawdzenie co sie po nim stalo odbywa sie w metodzie move 
		public string move(int pos, Player player)
		{
			string symbol = " ";
			if (!p1Positions.Contains(pos) && !p2Positions.Contains(pos))
			{
				if (player.Equals(p1))
				{
					symbol = "X";
					p1Positions.Add(pos);
				}
				else if (player.Equals(p2))
				{
					symbol = "0";
					p2Positions.Add(pos);
				}
				if (p1Positions.Contains(1) && p1Positions.Contains(2) && p1Positions.Contains(3)) return "p1 wins";
				if (p1Positions.Contains(4) && p1Positions.Contains(5) && p1Positions.Contains(6)) return "p1 wins";
				if (p1Positions.Contains(7) && p1Positions.Contains(8) && p1Positions.Contains(9)) return "p1 wins";
				if (p1Positions.Contains(1) && p1Positions.Contains(4) && p1Positions.Contains(7)) return "p1 wins";
				if (p1Positions.Contains(2) && p1Positions.Contains(5) && p1Positions.Contains(8)) return "p1 wins";
				if (p1Positions.Contains(3) && p1Positions.Contains(6) && p1Positions.Contains(9)) return "p1 wins";
				if (p1Positions.Contains(1) && p1Positions.Contains(5) && p1Positions.Contains(9)) return "p1 wins";
				if (p1Positions.Contains(7) && p1Positions.Contains(5) && p1Positions.Contains(3)) return "p1 wins";

				if (p2Positions.Contains(1) && p2Positions.Contains(2) && p2Positions.Contains(3)) return "p2 wins";
				if (p2Positions.Contains(4) && p2Positions.Contains(5) && p2Positions.Contains(6)) return "p2 wins";
				if (p2Positions.Contains(7) && p2Positions.Contains(8) && p2Positions.Contains(9)) return "p2 wins";
				if (p2Positions.Contains(1) && p2Positions.Contains(4) && p2Positions.Contains(7)) return "p2 wins";
				if (p2Positions.Contains(2) && p2Positions.Contains(5) && p2Positions.Contains(8)) return "p2 wins";
				if (p2Positions.Contains(3) && p2Positions.Contains(6) && p2Positions.Contains(9)) return "p2 wins";
				if (p2Positions.Contains(1) && p2Positions.Contains(5) && p2Positions.Contains(9)) return "p2 wins";
				if (p2Positions.Contains(7) && p2Positions.Contains(5) && p2Positions.Contains(3)) return "p2 wins";



				if (p2Positions.Count + p1Positions.Count == 9)
					return "it's a tie";
				else
					return "nobody wins!";
			}
			else return "This position is taken";




		}
	}
}
