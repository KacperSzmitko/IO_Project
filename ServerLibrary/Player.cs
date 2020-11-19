using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ServerLibrary
{
    class Player
    {
        public IPAddress addres { get; set; }
        public  int sessionId { get; set; }
        public string name { get; set; }
        
        //Index to Games array
        public int match_id { get; set; }

        Player(IPAddress addres)
        {
            this.addres = addres;
            this.sessionId = -1;
            this.name = null;
            this.match_id = -1;
        }
    }
}
