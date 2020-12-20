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

        private static readonly object functionLock = new object();
        private static readonly object playersLock = new object();
        private static readonly object gamesLock = new object();
        private static readonly object playersWaitingForGameLock = new object();


        private Security security;

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

            lock (functionLock)
            {
                Console.WriteLine(System.Threading.Thread.CurrentThread.ManagedThreadId);
                return functions[option](string.Join("$$", list), clientID);
            }

        }


        //TODO //End game and set this client to lose
        private string Logout(string msg, int clientID)
        {
            
            if (players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NOT_LOGGED_IN, (int)Options.LOGOUT);
            lock (playersLock)
            {
                if(players[clientID].matchID > 0)
                {
                    lock (gamesLock)
                    {
                        //End game and set this client to lose

                    }
                }
                players[clientID].sessionId = null;
            }
            return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.LOGOUT);
        }

        //Create answer with client match history
        private string MatchHistory(string msg, int clientID)
        {
            
            string playerName = "";
            lock (playersLock)
            {
                if (players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NOT_LOGGED_IN, (int)Options.MATCH_HISTORY);
                try
                {
                    playerName = players[clientID].name;
                }
                catch
                {
                    return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.USER_NOT_FOUND, (int)Options.MATCH_HISTORY);
                }
            }
            
            try { return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.MATCH_HISTORY, dbConnection.GetMatchHistoryData(playerName)); }
            catch { return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.DB_CONNECTION_ERROR, (int)Options.MATCH_HISTORY); }
        }


        private string Rank(string msg, int clientID)
        {
            
            lock (playersLock) if (players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NOT_LOGGED_IN, (int)Options.RANK);
            try { return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.RANK, dbConnection.GetRankData()); }
            catch(Exception e) { return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.DB_CONNECTION_ERROR, (int)Options.RANK, e.Message); }
        }


        private string SearchGame(string msg, int clientID)
        {
            
            lock (playersLock) if (players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NOT_LOGGED_IN, (int)Options.SEARCH_GAME);
            lock (playersWaitingForGameLock)
            {
                if(playersWaitingForGame.Contains(clientID))
                {
                    return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.GAME_IS_ALREADY_SEARCHED, (int)Options.SEARCH_GAME);
                }
                playersWaitingForGame.Add(clientID);
            }
            //lock (playersLock) players[clientID].matchID = -1;
            return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.SEARCH_GAME);
        }


        // Try to login client
        //TODO Add username to user 
        public string Login(string msg, int clientID)
        {

            string[] fields = msg.Split("$$", StringSplitOptions.RemoveEmptyEntries);
            string username = fields[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            string password = fields[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];

            string sessionId = "";

            string passwordHash = "";
            try { passwordHash = dbConnection.GetUserPassword(username); }
            catch  { return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.USER_NOT_FOUND, (int)Options.LOGIN); }

            if (security.VerifyPassword(passwordHash, password))
            {
                string elo = "";
                lock (playersLock)
                {
                    foreach (Player p in players)
                    {
                        if (p != null)
                        {
                            if (p.name == username && p.sessionId != null)
                            {
                                return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.USER_ALREADY_LOGGED_IN, (int)Options.LOGIN);
                            }
                        }
                    }

                    if (players[clientID].sessionId == null)
                    {
                        players[clientID].GenerateSessionId();
                        players[clientID].elo = dbConnection.GetClientElo(username);
                        elo = players[clientID].elo.ToString();
                        players[clientID].name = username;
                    }
                    sessionId = players[clientID].sessionId;
                }
                return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.LOGIN, sessionId, elo);
            }
            else return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.INCORRECT_PASSWORD, (int)Options.LOGIN);
        }

        // Try to create new user 
        public string CreateUser(string msg, int clientID)
        {
            
            string[] fields = msg.Split("$$", StringSplitOptions.RemoveEmptyEntries);
            string username = fields[0].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            string password = fields[1].Split(':', StringSplitOptions.RemoveEmptyEntries)[1];
            if(dbConnection.CheckIfNameExist(username))
                return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.USER_ALREADY_EXISTS, (int)Options.CREATE_USER);

            password = security.HashPassword(password);
            if (dbConnection.AddNewUser(username, password)) return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.CREATE_USER);
            else return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.CREATE_USER);
        }

       

        public string Disconnect(string msg,int clientID)
        {      
            lock (playersLock)
            {
                players[clientID] = null;
                return "";
            }
        }


        public string CheckUserName(string msg,int clientID)
        {
            
            string[] fields = msg.Split("$$");
            string username = fields[0].Split(':')[1];
            if (!dbConnection.CheckIfNameExist(username))
                return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.CHECK_USER_NAME);
            return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.USER_ALREADY_EXISTS, (int)Options.CHECK_USER_NAME);
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
                lock(playersWaitingForGameLock) playersWaiting = new List<int>(playersWaitingForGame);
                lock(playersLock) players = new List<Player>(this.players);



                foreach (int waitingPlayer in playersWaiting)
                {
                    for (int i = 0; i < playersWaiting.Count; i++)
                    {
                        if (waitingPlayer != playersWaiting[i])
                        {
                            if (Math.Abs(players[waitingPlayer].elo - players[playersWaiting[i]].elo) <= 300)
                            {                                                                      
                                int matchID = games.Count;
                                lock (playersLock)
                                {
                                    this.players[waitingPlayer].matchID = matchID;
                                    this.players[waitingPlayer].matchActualization = true;
                                    this.players[playersWaiting[i]].matchID = matchID;
                                    this.players[playersWaiting[i]].matchActualization = true;
                                }
                                lock (gamesLock) games.Add(new Gameplay(this.players[waitingPlayer], this.players[playersWaiting[i]], 9, Variables.scoreToWin));


                                lock (playersWaitingForGameLock)
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
                Thread.Sleep(3000);
            }
        }

        //Send founded match info
        public string SendMatch(string msg,int clientID)
        {
            
            int matchID;
            string playerName;
            lock (playersLock)
            {
                matchID = players[clientID].matchID;
                playerName = players[clientID].name;
            }
            Player p1, p2;
            lock (gamesLock)
            {
                p1 = games[matchID].p1;
                p2 = games[matchID].p2;
                p1.playerTurn = true;

            }

            lock (playersLock)
            {
                if(p1.name == playerName)
                {
                    return (TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.SEARCH_GAME, p2.name, p2.elo.ToString(), "1"));
                }
                return (TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.SEARCH_GAME, p1.name, p1.elo.ToString(), "0"));
            }
        }

        public string SendOppMove(string msg,int clientID)
        {
            lock (gamesLock)
            {
                    string name = players[clientID].name;
                    int p1Points = games[players[clientID].matchID].p1Points;
                    int p2Points = games[players[clientID].matchID].p2Points;
                    if(players[clientID].name == games[players[clientID].matchID].p1.name)
                    return (TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.OPP_MOVE,
                            String.Format("{0}-{1}",p1Points,p2Points),games[players[clientID].matchID].lastMove.ToString()));
                    return (TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.OPP_MOVE,
                            String.Format("{1}-{0}", p1Points, p2Points), games[players[clientID].matchID].lastMove.ToString()));
            }
        }

     
        private string SendMove(string msg, int clientID)
        {

            int matchID;
            lock (playersLock)
            {
                if (players[clientID].sessionId == null) return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NOT_LOGGED_IN, (int)Options.SEND_MOVE);
                if (players[clientID].matchID < 0) return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NOT_IN_GAME, (int)Options.SEND_MOVE);
                matchID = players[clientID].matchID;
            }
            string[] fields = msg.Split("$$", StringSplitOptions.RemoveEmptyEntries);
            string f = fields[1].Split(":", StringSplitOptions.RemoveEmptyEntries)[1];
            int move = Int32.Parse(f);


            lock (games[matchID])
            {
                lock (playersLock)
                {
                    bool check_move = games[matchID].move(move, players[clientID]);

                    //Correct move
                    if (check_move)
                    {
                        if (games[matchID].roundEnd) {
                            games[matchID].roundEnd = false;
                        }

                        if (players[clientID].name == games[matchID].p1.name)
                        {
                            games[matchID].p1.playerTurn = false;
                            games[matchID].p2.playerTurn = true;
                            //Player1 won game
                            if(games[matchID].p1Points >= games[matchID].pointsToWin )
                            {
                                int p1Elo = games[matchID].p1.elo;
                                int p2Elo = games[matchID].p2.elo;

                                games[matchID].p1.elo = (int)CalcElo(games[matchID].p1.elo, games[matchID].p2.elo, 1, 30);
                                games[matchID].p2.elo = (int)CalcElo(games[matchID].p2.elo, games[matchID].p1.elo, 0, 30);
                                dbConnection.EloUpdate(games[matchID].p1.name, games[matchID].p1.elo);
                                dbConnection.EloUpdate(games[matchID].p2.name, games[matchID].p2.elo);

                                dbConnection.EloUpdate(games[matchID].p1.name, games[matchID].p1.elo);
                                dbConnection.EloUpdate(games[matchID].p2.name, games[matchID].p2.elo);
                                Gameplay game = games[players[clientID].matchID];

                                dbConnection.MatchHistoryUpdate(game.p1.name, game.p2.name, game.p1.name, game.p1.elo.ToString(), game.p2.elo.ToString(),
                                    (p1Elo - game.p1.elo).ToString(), (p2Elo - game.p2.elo).ToString(), game.p1Points.ToString(), game.p2Points.ToString());

                                games[matchID].p1.won = true;
                                games[matchID].p2.lose = true;
                            }
                        }
                        else
                        {
                            games[matchID].p1.playerTurn = true;
                            games[matchID].p2.playerTurn = false;
                            //Player2 won game
                            if (games[matchID].p2Points >= games[matchID].pointsToWin)
                            {
                                int p1Elo = games[matchID].p1.elo;
                                int p2Elo = games[matchID].p2.elo;
                                games[matchID].p1.elo = (int)CalcElo(games[matchID].p1.elo, games[matchID].p2.elo, 0, 30);
                                games[matchID].p2.elo = (int)CalcElo(games[matchID].p2.elo, games[matchID].p1.elo, 1, 30);

                                dbConnection.EloUpdate(games[matchID].p1.name, games[matchID].p1.elo);
                                dbConnection.EloUpdate(games[matchID].p2.name, games[matchID].p2.elo);
                                Gameplay game = games[players[clientID].matchID];

                                dbConnection.MatchHistoryUpdate(game.p1.name, game.p2.name, game.p2.name, game.p1.elo.ToString(), game.p2.elo.ToString(),
                                    (p1Elo - game.p1.elo).ToString(), (p2Elo - game.p2.elo).ToString(), game.p1Points.ToString(), game.p2Points.ToString());

                                games[matchID].p1.lose = true;
                                games[matchID].p2.won = true;
                            }
                        }


                        games[matchID].lastMove = move;
                        if (players[clientID].name == games[matchID].p1.name)
                            return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.SEND_MOVE, String.Format("{0}-{1}",
                                games[matchID].p1Points, games[matchID].p2Points));
                        else return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.SEND_MOVE, String.Format("{1}-{0}",
                            games[matchID].p1Points, games[matchID].p2Points));
                    }
                    return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.MOVE_NOT_ALLOWED, (int)Options.SEND_MOVE, "Niedozwolony ruch");
                }
            }
        }

        public string EndGameMessage(string msg, int clientID)
        {
            lock (gamesLock)
            {
                lock (playersLock)
                {
                    if(games[players[clientID].matchID].p1.name == players[clientID].name)
                    {
                        return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.END_GAME, games[players[clientID].matchID].p1.elo.ToString(),
                            games[players[clientID].matchID].p2.elo.ToString());
                    }
                    else
                    {
                        return TransmisionProtocol.CreateServerMessage((int)ErrorCodes.NO_ERROR, (int)Options.END_GAME, games[players[clientID].matchID].p2.elo.ToString(),
                            games[players[clientID].matchID].p1.elo.ToString());
                    }
                }
            }
        }

        //Returns true if game has been founded for client
        public bool CheckMatchAcctualization(int clientID)
        {
            lock (playersLock)
            {
                try
                {
                    if (players[clientID].matchActualization)
                    {
                        players[clientID].matchActualization = false;
                        return true;
                    }
                }
                catch (NullReferenceException)
                {
                    throw new Exception();
                }
                return false;
            }
        }

        public int AddPlayer(Player player)
        {
            players.Add(player);
            return players.Count-1;
        }

        public bool CheckPlayerTurn(int clientID)
        {
            lock (playersLock)
            {
                lock (gamesLock)
                {
                    if (players[clientID].playerTurn && !games[players[clientID].matchID].roundEnd) {
                        players[clientID].playerTurn = false;
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientID"></param>
        /// <returns>0 - No end game  1 - Player won game  2 - Player lost game</returns>
        public int CheckEndGame(int clientID)
        {
            lock (playersLock)
            {
                lock (gamesLock)
                {
                    if (players[clientID].won)
                    {
                        players[clientID].won = false;
                        return 1;
                    }
                    else if (players[clientID].lose)
                    { 
                        players[clientID].lose = false;
                        return 2;
                    }
                    return 0;
                    
                }
            }
        }

        public double CalcElo(double playerElo,double oppElo,int won,int k)
        {
            double p1 = (1.0/(1.0 + Math.Pow(10,(oppElo - playerElo)/400.0)));
            return playerElo + k * (won - p1);
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
            functions.Add(new Functions(EndGameMessage));
            functions.Add(new Functions(Login));
            functions.Add(new Functions(CreateUser));
            functions.Add(new Functions(SendMove));
            functions.Add(new Functions(Disconnect));
            functions.Add(new Functions(CheckUserName));
            functions.Add(new Functions(SendMatch));
            functions.Add(new Functions(SendOppMove));

            matchMaking = new Thread(MatchMaking);
            security = new pbkdf2();
            matchMaking.Start();
        }
    }
}
