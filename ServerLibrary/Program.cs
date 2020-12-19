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
            DbMethods db = new DbMethods();
            db.MatchHistoryUpdate("test", "test2", "test", "1200", "1400", "23", "56", "5", "3");

            //ServerConnection server = new ServerConnection();
            //server.RunServer();
        }
    }
}
