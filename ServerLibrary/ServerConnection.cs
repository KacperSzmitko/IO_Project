using System;
using System.Collections.Generic;
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
                //Thread t = new Thread(ClientConnection);
                //t.Start(client);
                Task.Run(() => { ClientConnectionAsync(client); });
            }
        }

        public async void ClientConnectionAsync(Object obj)
        {
            TcpClient client = obj as TcpClient;
            NetworkStream stream = client.GetStream();
            stream.ReadTimeout = 5000;
            byte[] message;

            int clientID = menager.AddPlayer(new Player());
            while (true)
            {
                //Read message
                string sendMessage = "";
                byte[] buffer = new byte[2048];
                StringBuilder messageData = new StringBuilder();
                int bytes = -1;
                try
                {
                    bytes = await stream.ReadAsync(buffer, 0, buffer.Length);
                }
                catch
                {
                    //Found game
                    if (menager.CheckMatchAcctualization(clientID))
                    {
                        sendMessage = menager.ProccesClient("Option:8$$", clientID);
                        message = Encoding.ASCII.GetBytes(sendMessage);
                        stream.Write(message);
                    }
                    continue;
                }

                //Decode message
                Decoder decoder = Encoding.ASCII.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);

                //Prepare response
                sendMessage = menager.ProccesClient(messageData.ToString(), clientID);

                //Disconnection
                if (sendMessage == "")
                {
                    message = Encoding.ASCII.GetBytes("Response:True$$");
                    stream.Write(message);
                    break;
                }
                message = Encoding.ASCII.GetBytes(sendMessage);

                //Send response
                stream.Write(message);
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
            stream.ReadTimeout = 5000;
            byte[] message;

            int clientID = menager.AddPlayer(new Player());
            while (true)
            {
                //Read message
                string sendMessage = "";
                byte[] buffer = new byte[2048];
                StringBuilder messageData = new StringBuilder();
                int bytes = -1;
                try
                {
                    bytes = stream.Read(buffer, 0, buffer.Length);
                }
                catch
                {
                    //Found game
                    if (menager.CheckMatchAcctualization(clientID))
                    {
                        sendMessage = menager.ProccesClient("Option:8$$", clientID);
                        message = Encoding.ASCII.GetBytes(sendMessage);
                        stream.Write(message);
                    }
                    continue;
                }

                //Decode message
                Decoder decoder = Encoding.ASCII.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);

                //Prepare response
                sendMessage = menager.ProccesClient(messageData.ToString(), clientID);

                //Disconnection
                if(sendMessage == "")
                {
                    message = Encoding.ASCII.GetBytes("Response:True$$");
                    stream.Write(message);
                    break;
                }
                message = Encoding.ASCII.GetBytes(sendMessage);

                //Send response
                stream.Write(message);
            }
        }

        public void test(List<string> commands)
        {
            int clientID = menager.AddPlayer(new Player());

            foreach (string command in commands)
            {
                Console.WriteLine(command);
                string sendMessage = "";
                sendMessage = menager.ProccesClient(command, clientID);
                Console.WriteLine(sendMessage);
                sendMessage = "";

            }
        }

        public ServerConnection()
        {
            menager = new ClientProcesing();
        }


    }
}
