using System;
using System.Collections.Generic;
using System.Net;

namespace ServerLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string g = TransmisionProtocol.CreateServerMessage(1, 1, "Elo");
            //ServerConnection server = new ServerConnection();
            //server.RunServer();
        }
    }
}
