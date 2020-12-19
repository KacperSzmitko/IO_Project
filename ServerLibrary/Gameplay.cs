using System;
using System.Collections.Generic;
using System.Text;


namespace ServerLibrary
{
	
    public class Gameplay
    {
		//p1 starts 
        public Player p1{get; set;}
        public Player p2{get; set;}
        public int p1Points { get; set; }
        public int p2Points { get; set; }
        public int pointsToWin { get; set; }
		public bool roundEnd { get; set; }
		public int lastMove { get; set; }
		public List<int> p1Positions { get; set; }
		public List<int> p2Positions { get; set; }


		public Gameplay(Player p1,Player p2,int boardSize,int pointsToWin)
        {
            this.p1 = p1;
            this.p2 = p2;
			this.roundEnd = true;
            this.pointsToWin = pointsToWin;
            p1Points = 0;
            p2Points = 0;

			p1Positions = new List<int>();
			p2Positions = new List<int>();

		}
		// zapisanie danego ruchu i sprawdzenie co sie po nim stalo odbywa sie w metodzie move 
		public bool move(int pos, Player player)
		{
			if (!p1Positions.Contains(pos) && !p2Positions.Contains(pos))
			{
				if (player.Equals(p1))
				{
					p1Positions.Add(pos);
				}
				else if (player.Equals(p2))
				{
					p2Positions.Add(pos);
				}
				if (p1Positions.Contains(0) && p1Positions.Contains(1) && p1Positions.Contains(2)) {
                    p1Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true; }
				if (p1Positions.Contains(3) && p1Positions.Contains(4) && p1Positions.Contains(5))
				{
					p1Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p1Positions.Contains(6) && p1Positions.Contains(7) && p1Positions.Contains(8))
				{
					p1Points++; roundEnd = true;
					p1Positions.Clear();																																																																																																	
					p2Positions.Clear();
					return true;
				}
				if (p1Positions.Contains(0) && p1Positions.Contains(3) && p1Positions.Contains(6))
				{
					p1Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p1Positions.Contains(1) && p1Positions.Contains(4) && p1Positions.Contains(7))
				{
					p1Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p1Positions.Contains(2) && p1Positions.Contains(5) && p1Positions.Contains(8))
				{
					p1Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p1Positions.Contains(0) && p1Positions.Contains(4) && p1Positions.Contains(8))
				{
					p1Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p1Positions.Contains(6) && p1Positions.Contains(4) && p1Positions.Contains(2))
				{
					p1Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}



				if (p2Positions.Contains(0) && p2Positions.Contains(1) && p2Positions.Contains(2))
				{
					p2Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p2Positions.Contains(3) && p2Positions.Contains(4) && p2Positions.Contains(5))
				{
					p2Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p2Positions.Contains(6) && p2Positions.Contains(7) && p2Positions.Contains(8))
				{
					p2Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p2Positions.Contains(0) && p2Positions.Contains(3) && p2Positions.Contains(6))
				{
					p2Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p2Positions.Contains(1) && p2Positions.Contains(4) && p2Positions.Contains(7))
				{
					p2Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p2Positions.Contains(2) && p2Positions.Contains(5) && p2Positions.Contains(8))
				{
					p2Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p2Positions.Contains(0) && p2Positions.Contains(4) && p2Positions.Contains(8))
				{
					p2Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				if (p2Positions.Contains(6) && p2Positions.Contains(4) && p2Positions.Contains(2))
				{
					p2Points++; roundEnd = true;
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}

				if (p2Positions.Count + p1Positions.Count == 9) 
				{
					p1Positions.Clear();
					p2Positions.Clear();
					return true;
				}
				else return true;
			}
			else return false; //pozycja zajeta, ruch niedozwolony
		}
	}
}
