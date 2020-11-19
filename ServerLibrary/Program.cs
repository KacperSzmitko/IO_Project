using System;

namespace ServerLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
    
            ServerConnection.RunServer("./cert.cer");
        }
    }
}
