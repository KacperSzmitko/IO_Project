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
            ClientProcesing cp = new ClientProcesing();
            Console.WriteLine((int)cp.CalcElo(1200, 1000, 1, 30));

            //ServerConnection server = new ServerConnection();
            //server.RunServer();
        }
    }
}
