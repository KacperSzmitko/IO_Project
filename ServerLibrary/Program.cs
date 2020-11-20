using System;
using System.Net;

namespace ServerLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            Security.MakeCert();
            ServerConnection server = new ServerConnection();
           
            
            
            
            
            server.RunServer("./cert.cer");
        }
    }
}
