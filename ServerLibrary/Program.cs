using System;
using System.Collections.Generic;
using System.Net;

namespace ServerLibrary
{
    class Program
    {
        static void Main(string[] args)
        {

            ClientProcesing cp = new ClientProcesing();
            ServerLibrary.Player p = new ServerLibrary.Player();
            p.matchID = 0;

            int id = cp.AddPlayer(p);
            string response = cp.ProccesClient("Option:5$$Username:tes$$Password:1234$$", id);
            Console.WriteLine(response);
            string[] r = response.Split("$$");
            string sessionID = r[1].Split(":")[1];
            p.sessionId = sessionID;
            cp.games.Add(new Gameplay(p, new Player(), 9, 5));
            response = String.Format("Option:7$$SessionID:{0}$$Move:1$$", sessionID);
            response = cp.ProccesClient(response, id);
            Console.WriteLine(response);



        }
    }
}
