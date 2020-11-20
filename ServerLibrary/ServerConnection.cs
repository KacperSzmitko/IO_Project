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

namespace ServerLibrary
{


    class ServerConnection
    {
        public delegate void TransmissionDataDelegate(SslStream stream);
        public ClientProcesing menager {get;set;}
        public void RunServer(string certificate)
        {
            // Create a TCP/IP (IPv4) socket and listen for incoming connections.
            TcpListener listener = new TcpListener(IPAddress.Any, 17777);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                TcpClient client = listener.AcceptTcpClient();
                AuthClient(client);
            }
        }

        private void TransmissionCallback(IAsyncResult result) {
            var tcpClient = result.AsyncState as TcpClient;
            tcpClient.Close();
        }

        public void AuthClient(TcpClient client) 
        {

            // A client has connected. Create the
            // SslStream using the client's network stream.
            SslStream sslStream = new SslStream(client.GetStream(), false);
            TransmissionDataDelegate transmissionDelegate = new TransmissionDataDelegate(OnAuthenticated);
            transmissionDelegate.BeginInvoke(sslStream, TransmissionCallback, client);
            // Authenticate the server but don't require the client to authenticate.
        }
        //Function invoked after authentication as server
        protected void OnAuthenticated(SslStream sslStream)
        {
            
            int playerID = menager.AddPlayer(new Player());
            string sendMessage = "";
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;


            bytes = sslStream.Read(buffer, 0, buffer.Length);

            Decoder decoder = Encoding.ASCII.GetDecoder();
            char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
            decoder.GetChars(buffer, 0, bytes, chars, 0);
            messageData.Append(chars);


            Console.WriteLine(messageData);

            //sendMessage = menager.ProccesClient(messageData.ToString(), playerID);
            byte[] message = Encoding.ASCII.GetBytes("PYK");
            sslStream.Write(message);
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

                // Use Decoder class to convert from bytes to ASCII
                // in case a character spans two buffers.
                Decoder decoder = Encoding.ASCII.GetDecoder();
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
