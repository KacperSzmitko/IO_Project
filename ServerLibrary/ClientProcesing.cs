using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ServerLibrary
{
    /// <summary>
    /// Class which process all client messages sended here from ServerConnection,
    /// and gives back server response ready to send to client
    /// </summary>
    class ClientProcesing
    {
        //TODO SearchGame  EndGame  SendMove

        /// <summary>
        /// Delegate which represents function used to procces client data
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="clientID"></param>
        /// <returns></returns>
        public delegate string Functions(string msg, int clientID);
        private Thread matchMaking;

        /// <summary>
        /// List of all avaliable functions, index of given function
        /// is number of that option
        /// </summary>
        public List<Functions> functions { get; set; }

        public List<Player> players { get; set; }

        public List<Gameplay> games { get; set; }

        public List<int> playersWaitingForGame { get; set; }

        public DbConnection dbConnection { get; set; }

        

        /// <summary>
        /// Function that takes message from client procces it and return server response
        /// </summary>
        /// <param name="message"> Client message</param>
        /// <returns>Server response ready to send</returns>
        public string ProccesClient(string message,int clientID)
        {
            string[] fields = message.Split("$$");
            int option = Int32.Parse(fields[0].Split(':')[1]);

            //Remove option
            var list = new List<string>(fields);
            list.RemoveAt(0);

            lock (functions)
            {
                return functions[option](string.Join("",list), clientID);
            }
        }


        //TODO //End game and set this client to lose
        private string Logout(string msg, int clientID)
        {
            bool succes = true;
            lock(players)
            {
                if(players[clientID].matchID != 0)
                {
                    //End game and set this client to lose
                }
                try
                {
                    players.RemoveAt(clientID);
                }
                catch(ArgumentOutOfRangeException err)
                {
                    succes = false;
                    return TransmisionProtocol.CreateServerMessage(succes, 0,err.Message);
                }
            }
            return TransmisionProtocol.CreateServerMessage(succes, 0);
        }

        //Create answer with client match history
        private string MatchHistory(string msg, int clientID)
        {
            bool succes = true;
            string playerName = "";
            lock (players)
            {
                try
                {
                    playerName = players[clientID].name;
                }
                catch
                {
                    succes = false;
                    return TransmisionProtocol.CreateServerMessage(succes, 1,"Nie znaleziono uzytkownika!");
                }
            }
            try { return TransmisionProtocol.CreateServerMessage(succes, 1, dbConnection.GetMatchHistoryData(playerName)); }
            catch(Exception e) { return TransmisionProtocol.CreateServerMessage(!succes, 1,e.Message); }
        }


        private string Rank(string msg, int clientID)
        {
            bool succes = true;
            try { return TransmisionProtocol.CreateServerMessage(succes, 2, dbConnection.GetRankData()); }
            catch(Exception e) { return TransmisionProtocol.CreateServerMessage(!succes, 2,e.Message); }
        }


        private string SearchGame(string msg, int clientID)
        {
            bool succes = true;

            lock(playersWaitingForGame)
            {
                if(playersWaitingForGame.Contains(clientID))
                {
                    return TransmisionProtocol.CreateServerMessage(!succes, 3,"Juz wyszukujesz rozgrywkie");
                }
                playersWaitingForGame.Add(clientID);
            }
            lock (players) players[clientID].matchID = -1;
            return TransmisionProtocol.CreateServerMessage(succes, 3);
        }

        //Returns true if game has been founded for client
        public bool CheckMatchAcctualization(int clientID)
        {
    
            lock (players)
            {
                if(players[clientID].matchActualization)
                {
                    players[clientID].matchActualization = false;
                    return true;
                }
                return false;
            }

        }

        public void MatchMaking()
        {
            bool found;
            List<int> playersWaiting;
            List<Player> players;
            while (true)
            {
                found = false;
                playersWaiting = new List<int>(playersWaitingForGame);
                players = new List<Player>(this.players);

            
                foreach(int waitingPlayer in playersWaiting)
                {
                    for(int i=0;i<playersWaiting.Count;i++)
                    {
                        if(waitingPlayer != playersWaiting[i])
                        {
                            if(Math.Abs(players[waitingPlayer].elo - players[playersWaiting[i]].elo) <= 150)
                            {
                                lock(this.players)
                                {
                                    lock (games)
                                    {
                                        int matchID = games.Count;
                                        this.players[waitingPlayer].matchID = matchID;
                                        this.players[waitingPlayer].matchActualization = true;
                                        this.players[playersWaiting[i]].matchID = matchID;
                                        this.players[playersWaiting[i]].matchActualization = true;

                                        games.Add(new Gameplay(this.players[waitingPlayer], this.players[playersWaiting[i]], 9, 5));
                                    }

                                    var itemToRemove = this.playersWaitingForGame.Single(r => r == waitingPlayer);
                                    this.playersWaitingForGame.Remove(itemToRemove);

                                    itemToRemove = this.playersWaitingForGame.Single(r => r == playersWaitingForGame[i]);
                                    this.playersWaitingForGame.Remove(itemToRemove);

                                    found = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (found) break;
                }
                Thread.Sleep(5000);
            }
        }

        private string EndGame(string msg, int clientID)
        {
            bool succes = true;
            return TransmisionProtocol.CreateServerMessage(succes, 4);
        }

        // Try to login client
        //TODO Add username to user 
        public string Login(string msg, int clientID)
        {
            bool succes = true;
            string[] fields = msg.Split("$$");
            string username = fields[0].Split(':')[1];
            string password = fields[1].Split(':')[1];

            string sessionId = "";

            string passwordHash = "";
            try { passwordHash = dbConnection.GetUserPassword(username); }
            catch(Exception e) { return TransmisionProtocol.CreateServerMessage(!succes, 5,e.Message); }

            if (Security.VerifyPassword(passwordHash, password))
            {
                lock (players)
                {
                    if (players[clientID].sessionId == null)
                    {
                        players[clientID].GenerateSessionId();
                        players[clientID].elo = dbConnection.GetClientElo(username);
                    }
                    else return (TransmisionProtocol.CreateServerMessage(!succes, 5, "Ten uzytkownik jest juz zalogowany"));
                    sessionId = players[clientID].sessionId;
                }
                return TransmisionProtocol.CreateServerMessage(succes, 5, sessionId);
            }
            else return TransmisionProtocol.CreateServerMessage(!succes, 5,"Haslo nieprawidlowe");
        }

        // Try to create new user 
        public string CreateUser(string msg, int clientID)
        {
            bool succes = true;
            string[] fields = msg.Split("$$");
            string username = fields[0].Split(':')[1];
            string password = fields[1].Split(':')[1];
            if(dbConnection.CheckIfNameExist(username))
                return TransmisionProtocol.CreateServerMessage(!succes, 6,"Juz istnieje taki uzytkownik!");

            password = Security.HashPassword(password);
            if (dbConnection.AddNewUser(username, password)) return TransmisionProtocol.CreateServerMessage(succes, 6);
            else return TransmisionProtocol.CreateServerMessage(!succes, 6,"Nie udalo sie utworzyc uzytkownika!");
        }

        private string SendMove(string msg, int clientID)
        {
            bool succes = true;
            return msg;
        }

        public string SendMatch(string msg,int clientID)
        {
            bool succes = true;
            int matchID;
            string playerName;
            lock(players)
            {
                matchID = players[clientID].matchID;
                playerName = players[clientID].name;
            }
            Player p1, p2;
            lock(games)
            {
                p1 = games[matchID].p1;
                p2 = games[matchID].p2;
            }
            lock(players)
            {
                if(p1.name == playerName)
                {
                    return (TransmisionProtocol.CreateServerMessage(succes, 4, p2.name, p2.elo.ToString(), p1.elo.ToString()));
                }
                return (TransmisionProtocol.CreateServerMessage(succes, 4, p1.name, p1.elo.ToString(), p2.elo.ToString()));
            }
        }

        

        public int AddPlayer(Player player)
        {
            players.Add(player);
            return players.Count-1;
        }



        public ClientProcesing()
        {
            players = new List<Player>();
            games = new List<Gameplay>();
            playersWaitingForGame = new List<int>();
            functions = new List<Functions>();
            dbConnection = new DbConnection();
            functions.Add(new Functions(Logout));
            functions.Add(new Functions(MatchHistory));
            functions.Add(new Functions(Rank));
            functions.Add(new Functions(SearchGame));
            functions.Add(new Functions(EndGame));
            functions.Add(new Functions(Login));
            functions.Add(new Functions(CreateUser));
            functions.Add(new Functions(SendMove));
            functions.Add(new Functions(SendMatch));
            matchMaking = new Thread(MatchMaking);
            matchMaking.Start();
        }
    }
}
