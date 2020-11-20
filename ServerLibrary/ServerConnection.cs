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
        static X509Certificate serverCertificate = null;
        public ClientProcesing menager {get;set;}
        public void RunServer(string certificate)
        {
            serverCertificate = X509Certificate.CreateFromCertFile(certificate);
            // Create a TCP/IP (IPv4) socket and listen for incoming connections.
            TcpListener listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();
            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                TcpClient client = listener.AcceptTcpClient();
                AuthClient(client);
            }
        }

        public void AuthClient(TcpClient client)
        {
           // A client has connected. Create the
           // SslStream using the client's network stream.
           SslStream sslStream = new SslStream(
                client.GetStream(), false);
            // Authenticate the server but don't require the client to authenticate.
            try
            {
                sslStream.BeginAuthenticateAsServer(serverCertificate, clientCertificateRequired: false, checkCertificateRevocation: true, OnAuthenticated, sslStream);
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                }
                Console.WriteLine("Authentication failed - closing the connection.");
                sslStream.Close();
                client.Close();
                return;
            }
        }

        //Function invoked after authentication as server
        protected void OnAuthenticated(IAsyncResult result)
        {
            var sslStream = result.AsyncState as SslStream;
            int playerID = menager.AddPlayer(new Player());
            string sendMessage = "";
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;


            bytes = sslStream.Read(buffer, 0, buffer.Length);

            Decoder decoder = Encoding.UTF8.GetDecoder();
            char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
            decoder.GetChars(buffer, 0, bytes, chars, 0);
            messageData.Append(chars);

            if (messageData.ToString().IndexOf("<EOF>") != -1)
            {
                return;
            }

            sendMessage = menager.ProccesClient(messageData.ToString(), playerID);
            byte[] message = Encoding.UTF8.GetBytes(sendMessage + "< EOF>");
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
