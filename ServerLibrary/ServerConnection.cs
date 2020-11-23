using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerLibrary
{

    class ServerConnection
    {
        public ClientProcesing menager {get;set;}
        public void RunServer()
        {
            // Create a TCP/IP (IPv4) socket and listen for incoming connections.
            TcpListener listener = new TcpListener(IPAddress.Any, 17777);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                TcpClient client = listener.AcceptTcpClient();
                Thread t = new Thread(ClientConnection);
                t.Start(client);

            }
        }
        /// <summary>
        /// Method used to read data from client and send answers from server
        /// </summary>
        /// <param name="obj"></param>
        public void ClientConnection(Object obj)
        {
            TcpClient client = obj as TcpClient;
            NetworkStream stream = client.GetStream();

            int clientID = menager.AddPlayer(new Player());
            while (true)
            {
                //Read message
                string sendMessage = "";
                byte[] buffer = new byte[2048];
                StringBuilder messageData = new StringBuilder();
                int bytes = -1;
                bytes = stream.Read(buffer, 0, buffer.Length);

                //Decode message
                Decoder decoder = Encoding.ASCII.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);

                //Prepare response
                sendMessage = menager.ProccesClient(messageData.ToString(), clientID);
                byte[] message = Encoding.ASCII.GetBytes(sendMessage);

                //Send response
                stream.Write(message);

                //Found game
                if(menager.CheckMatchAcctualization(clientID))
                {
                    sendMessage = menager.ProccesClient("Option:8$$", clientID);
                    message = Encoding.ASCII.GetBytes(sendMessage);
                }
                stream.Write(message);

            }
        }



        public ServerConnection()
        {
            menager = new ClientProcesing();
        }


    }
}
