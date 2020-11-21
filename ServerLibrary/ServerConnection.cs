using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerLibrary
{

    class ServerConnection
    {
        public delegate void ParameterizedThreadStart(TcpClient client);
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
                Thread t = new Thread(AuthClient);
                t.Start(client);

            }
        }

        public void AuthClient(Object obj)
        {
            TcpClient client = obj as TcpClient;
            NetworkStream stream = client.GetStream();

            int playerID = menager.AddPlayer(new Player());
            string sendMessage = "";
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;


            bytes = stream.Read(buffer, 0, buffer.Length);

            Decoder decoder = Encoding.UTF8.GetDecoder();
            char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
            decoder.GetChars(buffer, 0, bytes, chars, 0);
            messageData.Append(chars);

            if (messageData.ToString().IndexOf("<EOF>") != -1)
            {
                return;
            }

            //sendMessage = menager.ProccesClient(messageData.ToString(), playerID);
            byte[] message = Encoding.UTF8.GetBytes(sendMessage + "< EOF>");
            stream.Write(message);
        }

        //Function invoked after authentication as server
        protected void OnAuthenticated(IAsyncResult result)
        {
           
        }


        private string ReadMessage(SslStream sslStream)
        {
            // Read the  message sent by the client.
            // The client signals the end of the message using the
            // "<EOF>" marker.
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                // Read the client's test message.
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                // Use Decoder class to convert from bytes to UTF8
                // in case a character spans two buffers.
                Decoder decoder = Encoding.UTF8.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);
                // Check for EOF or an empty message.
                if (messageData.ToString().IndexOf("<EOF>") != -1)
                {
                    break;
                }
            } while (bytes != 0);

            return messageData.ToString();
        }

        public ServerConnection()
        {
            menager = new ClientProcesing();
        }


    }
}
