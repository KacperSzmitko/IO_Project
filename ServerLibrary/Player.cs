using DbLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerLibrary
{
    public class Player
    {
        private Random random;
        public IPAddress address { get; set; }
        public  string sessionId { get; set; }
        public string name { get; set; }
        
        public bool matchActualization { get; set; }
        public int elo { get; set; }

        //Tells if player won a game
        public bool won { get; set; }

        public bool lose { get; set; }
        //Tells whos turn is now
        public bool playerTurn { get; set; }
        //Index to Games array
        public int matchID { get; set; }

        public DbMethods dbConnection { get; set; }

        public void GenerateSessionId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            sessionId = new string(Enumerable.Repeat(chars, 30)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public Player()
        {
            this.dbConnection = new DbMethods();
            this.sessionId = null;
            this.name = null;
            //-1 not in match >=0 in match
            this.matchID = -1;
            this.random = new Random();
            this.elo = 0;
            this.matchActualization = false;
            this.playerTurn = false;
            this.won = false;
            this.lose = false;
        }
    }
}
