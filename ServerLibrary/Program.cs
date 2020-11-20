using System;
using System.Net;

namespace ServerLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerConnection server = new ServerConnection();
            string res = "";
            TransmisionProtocol.CreateClientMessage(ref res,5, "User","Passwd");
            Player p1 = new Player(IPAddress.Parse("100.7.12.32"));
            p1.functions[0].Invoke(res);
            //p1.ProccesClient(res);
            //server.RunServer("./cert.cer");
        }
    }
}
