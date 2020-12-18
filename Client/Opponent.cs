using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public class Opponent
    {
        private readonly string username;
        private readonly int elo;
        private readonly bool startsMatch;

        public string Username { get { return username; } }
        public int Elo { get { return elo; } }

        public bool StartsMatch { get { return startsMatch; } }

        public Opponent(string username, int elo, bool startsMatch) {
            this.username = username;
            this.elo = elo;
            this.startsMatch = startsMatch;
        }
    }
}