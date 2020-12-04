using ServerLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tic_Tac_Toe
{
    class StartServer
    {
        public static void Main(string[] args)
        {
            ServerConnection server = new ServerConnection();
            server.RunServer();
        }
    }
}
