using System.Text;
using System.Net.Sockets;

namespace Client 
{
    public class ServerConnection 
    {

        private const int bufferSize = 1024;

        private readonly TcpClient tcpClient;
        private readonly NetworkStream stream;

        public ServerConnection(string serverAddress, ushort serverPort) {
            this.tcpClient = new TcpClient(serverAddress, serverPort);
            this.stream = tcpClient.GetStream();
        }

        public void SendMessage(string message) {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            this.stream.Write(messageBytes);
            this.stream.Flush();
        }

        public string ReadMessage() {
            byte[] buffer = new byte[bufferSize];
            int messageLen = stream.Read(buffer, 0, bufferSize);
            if (messageLen > 0) return Encoding.ASCII.GetString(buffer);
            else return "";
        }

        public void CloseConnection() {
            this.stream.Close();
            this.tcpClient.Close(); 
        }

    }
}
