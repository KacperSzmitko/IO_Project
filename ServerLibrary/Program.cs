using DbLibrary;
using System;
using System.Collections.Generic;
using System.Net;

namespace ServerLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            DbMethods m = new DbMethods();
            Console.WriteLine(TransmisionProtocol.CreateServerMessage(0, 1, m.GetMatchHistoryData("test")));

            //ServerConnection server = new ServerConnection();
            //server.RunServer();
        }
    }
}
