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
    public class ServerConnection
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

            byte[] message;

            int clientID = menager.AddPlayer(new Player());
            Task.Run(() => { ServerMessagesAsync(stream,clientID); });
            while (true)
            {
                try
                {
                    //Read message
                    //CancellationTokenSource cts = new CancellationTokenSource(1000);
                    string sendMessage = "";
                    byte[] buffer = new byte[2048];
                    StringBuilder messageData = new StringBuilder();
                    int bytes = -1;

                    //bytes = await stream.ReadAsync(buffer, 0, buffer.Length, cts.Token);
                    bytes = await stream.ReadAsync(buffer, 0, buffer.Length);

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
                        message = Encoding.ASCII.GetBytes("Response:0$$");
                        stream.Write(message);
                        break;
                    }
                    message = Encoding.ASCII.GetBytes(sendMessage);

                    //Send response
                    stream.Write(message);
                }
                catch
                {
                    return;
                }
            }
        }


        public async void ServerMessagesAsync(Object obj,int clientID) 
        {
            NetworkStream stream = obj as NetworkStream;
            string sendMessage;
            byte[] message;
            while (true)
            {
                try
                {
                    //Check if match has been founded
                    if (menager.CheckMatchAcctualization(clientID))
                    {
                        sendMessage = menager.ProccesClient("Option:10$$", clientID);
                        message = Encoding.ASCII.GetBytes(sendMessage);
                        stream.Write(message);
                    }

                    bool sendMove = menager.CheckPlayerTurn(clientID);
                    int endGame = menager.CheckEndGame(clientID);
                    //Send opponent move

                    if (sendMove && endGame != 1)
                    {
                        sendMessage = menager.ProccesClient("Option:11$$", clientID);
                        message = Encoding.ASCII.GetBytes(sendMessage);
                        stream.Write(message);

                        if(endGame == 2)
                        {
                            sendMessage = menager.ProccesClient("Option:4$$", clientID);
                            message = Encoding.ASCII.GetBytes(sendMessage);
                            stream.Write(message);

                        }
                    }

                    if (endGame == 1)
                    {
                        sendMessage = menager.ProccesClient("Option:4$$", clientID);
                        message = Encoding.ASCII.GetBytes(sendMessage);
                        stream.Write(message);
                    }

                }
                catch
                {
                    return;
                }

            }
        }

     
        public ServerConnection()
        {
            menager = new ClientProcesing();
        }


    }
}
