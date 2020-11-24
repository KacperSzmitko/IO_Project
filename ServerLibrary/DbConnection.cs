using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using DbLibrary;
using MySql.Data.MySqlClient;

namespace ServerLibrary
{
    class DbConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;
        public DbConnection()
        {
            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            server = "10.8.0.1";
            database = "projekt_io";
            uid = "user_io";
            password = "DN8OHj8mUkNmXBRm";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
        }

        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                return false;
            }
        }


        public int GetClientElo(string username)
        {
            string query = "SELECT elo FROM user u INNER JOIN ranking r ON r.user_id = u.id " + String.Format("WHERE u.login = '{0}'", username);
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {
                    dataReader.Read();
                    string elo = dataReader.GetString(0);
                    dataReader.Close();
                    this.CloseConnection();
                    return Int32.Parse(elo);
                }
                catch
                {
                    throw new Exception("Nie ma takiego uzytkownika!");
                }
            }
            return 0;
        }

        public bool CheckIfNameExist(string username)
        {
            string query = string.Format("SELECT login FROM user WHERE login = '{0}'", username);
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {
                    dataReader.Read();
                    string password = dataReader.GetString(0);
                }
                catch
                {
                    this.CloseConnection();
                    return false;
                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();
                return true;
            }
            else
            {
                throw new Exception("Nie mozna polaczyc sie z baza danych");

            }
        }

        public string GetUserPassword(string username)
        {
            string query = "SELECT password FROM user " + String.Format("WHERE login = '{0}'", username);
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                try
                {
                    dataReader.Read();
                    string password = dataReader.GetString(0);
                    dataReader.Close();
                    this.CloseConnection();
                    return password;
                }
                catch
                {
                    throw new Exception("Nie ma takiego uzytkownika!");
                }
                //close Data Reader

                //close Connection

            }
            else
            {
                throw new Exception("Nie mozna polaczyc sie z baza danych");
            }
        }


        //Get player's match history as xml
        public string GetMatchHistoryData(string playerName)
        {
            string query = "SELECT mh.id,u.login,u2.login,u3.login,mh.end_time FROM match_history mh " +
                "INNER JOIN user u on u.id = mh.player1_id " +
                "INNER JOIN user u2 on u2.id = mh.player2_id " +
                "INNER JOIN user u3 on u3.id = mh.winner " +
                String.Format("WHERE u.login = '{0}' OR u2.login = '{1}' ", playerName, playerName) +
                "ORDER BY mh.end_time DESC";
            MatchHistory mh;
            List<MatchHistory> list = new List<MatchHistory>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    mh = new MatchHistory();
                    mh.id = dataReader.GetString(0);
                    mh.player1 = dataReader.GetString(1);
                    mh.player2 = dataReader.GetString(2);
                    mh.winner = dataReader.GetString(3);
                    mh.end_time = dataReader.GetString(4);
                    list.Add(mh);

                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();

                XmlDocument doc = new XmlDocument();
                XmlElement e = (XmlElement)doc.AppendChild(doc.CreateElement("Matches"));
                //Create xml from rows
                foreach (MatchHistory match in list)
                {
                    XmlElement el = (XmlElement)e.AppendChild(doc.CreateElement("Match"));
                    el.SetAttribute("Id", match.id);
                    el.AppendChild(doc.CreateElement("p1Name")).InnerText = match.player1;
                    el.AppendChild(doc.CreateElement("p2Name")).InnerText = match.player2;
                    el.AppendChild(doc.CreateElement("winnerName")).InnerText = match.winner;
                    el.AppendChild(doc.CreateElement("endTime")).InnerText = match.end_time;
                }
                //return list to be displayed
                return doc.OuterXml;
            }
            else
            {
                throw new Exception("Nie mozna polaczyc sie z baza danych");
            }
        }

        public string GetRankData()
        {
            string query = "SELECT r.id,u.login,r.elo " +
            "FROM ranking r " +
            "INNER JOIN user u ON u.id = r.user_id " +
            "ORDER BY r.elo DESC ";
            Rank rank;
            List<Rank> list = new List<Rank>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                while (dataReader.Read())
                {
                    rank = new Rank();
                    rank.id = dataReader.GetString(0);
                    rank.user = dataReader.GetString(1);
                    rank.elo = dataReader.GetString(2);
                    list.Add(rank);

                }
                //close Data Reader
                dataReader.Close();
                //close Connection
                this.CloseConnection();

                XmlDocument doc = new XmlDocument();
                XmlElement e = (XmlElement)doc.AppendChild(doc.CreateElement("RankTable"));
                //Create xml from rows
                int place = 1;
                foreach (Rank row in list)
                {
                    XmlElement el = (XmlElement)e.AppendChild(doc.CreateElement("Row"));
                    el.SetAttribute("Place", place.ToString());
                    el.AppendChild(doc.CreateElement("Player")).InnerText = row.user;
                    el.AppendChild(doc.CreateElement("Elo")).InnerText = row.elo;
                    place++;
                }
                //return list to be displayed
                return doc.OuterXml;
            }
            else
            {
                throw new Exception("Nie mozna polaczyc sie z baza danych");
            }
        }

        public bool AddNewUser(string username, string password)
        {
            string query = String.Format("INSERT INTO user(login,password) VALUES('{0}','{1}')", username, password);
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();
                dataReader.Close();

                query = String.Format("SELECT id FROM user WHERE user.login = '{0}'", username);
                cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                dataReader.Read();
                string userID = dataReader.GetString(0);
                dataReader.Close();

                query = String.Format("INSERT INTO ranking(user_id,elo) VALUES('{0}','{1}')", userID, 1000);
                cmd = new MySqlCommand(query, connection);
                dataReader = cmd.ExecuteReader();
                dataReader.Close();
                this.CloseConnection();
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
