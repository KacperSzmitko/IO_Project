using System;
using System.Collections.Generic;
using System.Net;

namespace ServerLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            
            ServerConnection server = new ServerConnection();
            server.RunServer();
            /*
            List<string> l = new List<string>
            {
                "Option:5$$Username:test$$Password:1234$$",
                "Option:2$$",
                "Option:0$$",
                "Option:2$$",
            };
            server.test(l);
            */
            

        }
    }
}
