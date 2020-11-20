using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Sockets;

namespace Client 
{
    public class ServerConnection 
    {

        private const int bufferSize = 1024;

        private static SslPolicyErrors _sslPolicyErrors;

        private readonly TcpClient tcpClient;
        private readonly SslStream sslStream;

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) {
            if (sslPolicyErrors == SslPolicyErrors.None) return true;
            _sslPolicyErrors = sslPolicyErrors;
            return false;
        }

        public ServerConnection(string serverAddress, ushort serverPort) {
            this.tcpClient = new TcpClient(serverAddress, 14000);
            this.sslStream = new SslStream(tcpClient.GetStream(), false);
        }

        public string GetSslErrors() {
            if (_sslPolicyErrors == SslPolicyErrors.None) return null;
            else return _sslPolicyErrors.ToString();
        }

        public void SendMessage(string message) {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            this.sslStream.Write(messageBytes);
            this.sslStream.Flush();
        }

        public string ReadMessage() {
            byte[] buffer = new byte[bufferSize];
            int messageLen = sslStream.Read(buffer, 0, bufferSize);
            if (messageLen > 0) return Encoding.ASCII.GetString(buffer);
            else return "";
        }

        public void CloseConnection() { this.tcpClient.Close(); }

    }
}
