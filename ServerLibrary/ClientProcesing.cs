using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLibrary
{
    class ClientProcesing
    {

        public delegate string Functions(string msg, int clientID);

        public Functions[] functions { get; set; }

        public List<Player> players { get; set; }

        public List<Gameplay> games { get; set; }

        public List<int> playersWaitingForGame { get; set; }

        /// <summary>
        /// Function that takes message from client procces it and return server response
        /// </summary>
        /// <param name="message"> Client message</param>
        /// <returns></returns>
        public string ProccesClient(string message,int clientID)
        {
            string[] fields = message.Split("$$");
            int option = Int32.Parse(fields[0].Split(':')[1]);
            return functions[option](message, clientID);
        }


        private string Logout(string msg, int clientID)
        {
            TransmisionProtocol.CreateServerMessage(ref msg, true, 0, players[clientID].sessionId.ToString());
            players.RemoveAt(clientID);
            return msg;
        }

        private string MatchHistory(string msg, int clientID)
        {
            return msg;
        }

        private string Rank(string msg, int clientID)
        {
            return msg;
        }

        private string SearchGame(string msg, int clientID)
        {
            return msg;
        }

        private string EndGame(string msg, int clientID)
        {
            return msg;
        }

        private string Login(string msg, int clientID)
        {
            return msg;
        }

        private string CreateUser(string msg, int clientID)
        {
            return msg;
        }
        private string SendMove(string msg, int clientID)
        {
            return msg;
        }

        public int AddPlayer(Player player)
        {
            players.Add(player);
            return players.Count;
        }

        public ClientProcesing()
        {
            players = new List<Player>();
            games = new List<Gameplay>();
            playersWaitingForGame = new List<int>();
            functions = new Functions[9];
            functions[0] = new Functions(Logout);
            functions[1] = new Functions(MatchHistory);
            functions[2] = new Functions(Rank);
            functions[3] = new Functions(SearchGame);
            functions[4] = new Functions(EndGame);
            functions[5] = new Functions(Login);
            functions[6] = new Functions(CreateUser);
            functions[7] = new Functions(SendMove);
        }
    }
}
