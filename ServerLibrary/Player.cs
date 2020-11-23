using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace ServerLibrary
{
    class Player
    {
        private Random random;
        public IPAddress address { get; set; }
        public  string sessionId { get; set; }
        public string name { get; set; }
        
        public bool matchActualization { get; set; }
        public int elo { get; set; }

        //Index to Games array
        public int matchID { get; set; }

        public void GenerateSessionId()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            sessionId = new string(Enumerable.Repeat(chars, 30)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public Player()
        {
            this.sessionId = null;
            this.name = null;
            //-2 doesnt search game -1 searching game >=0 in match
            this.matchID = -2;
            this.random = new Random();
            this.elo = 0;
            this.matchActualization = false;
        }
    }
}
