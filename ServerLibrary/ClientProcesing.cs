using DbLibrary;
using Shared;
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
    public class ClientProcesing
    {
        //TODO  EndGame  SendMove

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

        public DbMethods dbConnection { get; set; }

        

        /// <summary>
        /// Function that takes message from client procces it and return server response
        /// </summary>
        /// <param name="message"> Client message</param>
        /// <returns>Server response ready to send</returns>
        public string ProccesClient(string message,int clientID)
        {
            
            string[] fields = message.Split("$$", StringSplitOptions.RemoveEmptyEntries);
            int option = Int32.Parse(fields[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1]);

            //Remove option
            var list = new List<string>(fields);
            list.RemoveAt(0);

            lock (functions)
            {
                return functions[option](string.Join("$$",list), clientID);
            }
        }


        //TODO //End game and set this client to lose
        private string Logout(string msg, int clientID)
        {
            bool succes = true;
            if (players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.LOGOUT, "Nie jestes zalogowany");
            lock (players)
            {
                if(players[clientID].matchID > 0)
                {
                    lock (games)
                    {
                        //End game and set this client to lose

                    }
                }
                try
                {
                    players[clientID].sessionId = null;
                }
                catch(ArgumentOutOfRangeException err)
                {
                    succes = false;
                    return TransmisionProtocol.CreateServerMessage(succes, (int)Options.LOGOUT, err.Message);
                }
            }
            return TransmisionProtocol.CreateServerMessage(succes, (int)Options.LOGOUT);
        }

        //Create answer with client match history
        private string MatchHistory(string msg, int clientID)
        {
            bool succes = true;
            string playerName = "";
            lock (players)
            {
                if(players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.MATCH_HISTORY, "Nie jestes zalogowany");
                try
                {
                    playerName = players[clientID].name;
                }
                catch
                {
                    return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.MATCH_HISTORY, "Nie znaleziono uzytkownika!");
                }
            }
            try { return TransmisionProtocol.CreateServerMessage(succes, (int)Options.MATCH_HISTORY, dbConnection.GetMatchHistoryData(playerName)); }
            catch(Exception e) { return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.MATCH_HISTORY, e.Message); }
        }


        private string Rank(string msg, int clientID)
        {
            bool succes = true;
            lock (players) if (players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.RANK, "Nie jestes zalogowany");
            try { return TransmisionProtocol.CreateServerMessage(succes, (int)Options.RANK, dbConnection.GetRankData()); }
            catch(Exception e) { return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.RANK, e.Message); }
        }


        private string SearchGame(string msg, int clientID)
        {
            bool succes = true;
            lock (players) if (players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.SEARCH_GAME, "Nie jestes zalogowany");
            lock (playersWaitingForGame)
            {
                if(playersWaitingForGame.Contains(clientID))
                {
                    return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.SEARCH_GAME, "Juz wyszukujesz rozgrywkie");
                }
                playersWaitingForGame.Add(clientID);
            }
            lock (players) players[clientID].matchID = -1;
            return TransmisionProtocol.CreateServerMessage(succes, (int)Options.SEARCH_GAME);
        }

        private string EndGame(string msg, int clientID)
        {
            bool succes = true;
            return TransmisionProtocol.CreateServerMessage(succes, (int)Options.END_GAME);
        }

        // Try to login client
        //TODO Add username to user 
        public string Login(string msg, int clientID)
        {
            bool succes = true;
            string[] fields = msg.Split("$$", StringSplitOptions.RemoveEmptyEntries);
            string username = fields[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            string password = fields[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];

            string sessionId = "";

            string passwordHash = "";
            try { passwordHash = dbConnection.GetUserPassword(username); }
            catch(Exception e) { return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.LOGIN,e.Message); }

            if (Security.VerifyPassword(passwordHash, password))
            {
                string elo;
                lock (players)
                {
                    if (players[clientID].sessionId == null)
                    {
                        players[clientID].GenerateSessionId();
                        players[clientID].elo = dbConnection.GetClientElo(username);
                        elo = players[clientID].elo.ToString();
                    }
                    else return (TransmisionProtocol.CreateServerMessage(!succes, (int)Options.LOGIN, "Ten uzytkownik jest juz zalogowany"));
                    sessionId = players[clientID].sessionId;
                }
                return TransmisionProtocol.CreateServerMessage(succes, (int)Options.LOGIN, sessionId,elo);
            }
            else return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.LOGIN, "Haslo nieprawidlowe");
        }

        // Try to create new user 
        public string CreateUser(string msg, int clientID)
        {
            bool succes = true;
            string[] fields = msg.Split("$$", StringSplitOptions.RemoveEmptyEntries);
            string username = fields[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            string password = fields[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            if(dbConnection.CheckIfNameExist(username))
                return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.CREATE_USER,"Juz istnieje taki uzytkownik!");

            password = Security.HashPassword(password);
            if (dbConnection.AddNewUser(username, password)) return TransmisionProtocol.CreateServerMessage(succes, (int)Options.CREATE_USER);
            else return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.CREATE_USER, "Nie udalo sie utworzyc uzytkownika!");
        }

        private string SendMove(string msg, int clientID)
        {
            bool succes = true;
            int matchID;
            lock (players)
            {
                if(players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.SEND_MOVE, "Nie jestes zalogowany!");
                if(players[clientID].matchID < 0) return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.SEND_MOVE, "Nie jestes w rozgrywce");
                matchID = players[clientID].matchID;
            }
            string[] fields = msg.Split("$$", StringSplitOptions.RemoveEmptyEntries);
            string f = fields[1].Split(":", StringSplitOptions.RemoveEmptyEntries)[1];
            int move = Int32.Parse(f);

            
            lock(games[matchID])
            {
                lock (players)
                {
                    succes = games[matchID].move(move, players[clientID]);

                    if (succes)
                    {
                        if(players[clientID].name == games[matchID].p1.name)
                        return TransmisionProtocol.CreateServerMessage(succes, (int)Options.SEND_MOVE, String.Format("{0}-{1}",
                            games[matchID].p1Points, games[matchID].p2Points));
                        else return TransmisionProtocol.CreateServerMessage(succes, (int)Options.SEND_MOVE, String.Format("{1}-{0}",
                            games[matchID].p1Points, games[matchID].p2Points));
                    }
                    return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.SEND_MOVE, "Niedozwolony ruch");
                }
            }
        }

        public string Disconnect(string msg,int clientID)
        {
            bool succes = true;
            lock (players)
            {
                if (players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.DISCONNECT, "Nie jestes zalogowany");
                players.RemoveAt(clientID);
                return "";
            }

        }


        public string CheckUserName(string msg,int clientID)
        {
            bool succes = true;
            string[] fields = msg.Split("$$");
            string username = fields[0].Split(':')[1];
            if (dbConnection.CheckIfNameExist(username))
                return TransmisionProtocol.CreateServerMessage(!succes, (int)Options.CHECK_USER_NAME);
            return TransmisionProtocol.CreateServerMessage(succes, (int)Options.CHECK_USER_NAME);
        }

        public void MatchMaking()
        {
            bool found;
            List<int> playersWaiting;
            List<Player> players;
            while (true)
            {
                found = false;

                //Acutalization of using arrays
                playersWaiting = new List<int>(playersWaitingForGame);
                players = new List<Player>(this.players);


                foreach (int waitingPlayer in playersWaiting)
                {
                    for (int i = 0; i < playersWaiting.Count; i++)
                    {
                        if (waitingPlayer != playersWaiting[i])
                        {
                            if (Math.Abs(players[waitingPlayer].elo - players[playersWaiting[i]].elo) <= 150)
                            {                                                                      
                                int matchID = games.Count;
                                lock (this.players)
                                {
                                    this.players[waitingPlayer].matchID = matchID;
                                    this.players[waitingPlayer].matchActualization = true;
                                    this.players[playersWaiting[i]].matchID = matchID;
                                    this.players[playersWaiting[i]].matchActualization = true;
                                }
                                lock (games) games.Add(new Gameplay(this.players[waitingPlayer], this.players[playersWaiting[i]], 9, 5));


                                lock (this.playersWaitingForGame)
                                {
                                    var itemToRemove = this.playersWaitingForGame.Single(r => r == waitingPlayer);
                                    this.playersWaitingForGame.Remove(itemToRemove);

                                    itemToRemove = this.playersWaitingForGame.Single(r => r == playersWaiting[i]);
                                    this.playersWaitingForGame.Remove(itemToRemove);
                                }
                                found = true;
                                break;
                            }
                        }
                    }
                    if (found) break;
                }
                Thread.Sleep(5000);
            }
        }

        //Send founded match info
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
                    return (TransmisionProtocol.CreateServerMessage(succes, (int)Options.SEARCH_GAME, p2.name, p2.elo.ToString(), p1.elo.ToString()));
                }
                return (TransmisionProtocol.CreateServerMessage(succes, (int)Options.SEARCH_GAME, p1.name, p1.elo.ToString(), p2.elo.ToString()));
            }
        }



        //Returns true if game has been founded for client
        public bool CheckMatchAcctualization(int clientID)
        {
            lock (players)
            {
                if (players[clientID].matchActualization)
                {
                    players[clientID].matchActualization = false;
                    return true;
                }
                return false;
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
            dbConnection = new DbMethods();
            functions.Add(new Functions(Logout));
            functions.Add(new Functions(MatchHistory));
            functions.Add(new Functions(Rank));
            functions.Add(new Functions(SearchGame));
            functions.Add(new Functions(EndGame));
            functions.Add(new Functions(Login));
            functions.Add(new Functions(CreateUser));
            functions.Add(new Functions(SendMove));
            functions.Add(new Functions(Disconnect));
            functions.Add(new Functions(CheckUserName));
            functions.Add(new Functions(SendMatch));
            matchMaking = new Thread(MatchMaking);
            matchMaking.Start();
        }
    }
}
