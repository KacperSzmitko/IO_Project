using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class ServerCommands
    {
        private static string AddField(string fieldName, string value) {
            return fieldName + ":" + value + "$$";
        }

        /// <summary>
        /// Function which formats client data by using our transmition protocol
        /// </summary>
        /// <param name="result">Reference to string where u want your result to be stored</param>
        /// <param name="option">*0 - Logout  1 - MatchHistory  2 - Rank  3 - SearchGame  4 - EndGame  5 - Login  6 - CreateUser  7 - SendMove</param>
        /// <param name="fields">*0-4 : SessionID    5-6 : Username Password   7 : SessionID Move</param>
        public static string CreateClientMessage(int option, params string[] fields) {
            string result = "";
            try {
                result += AddField("option", option.ToString());
                //Logout MatchHistory Rank SearchGame EndGame
                if (option >= 0 && option <= 4) {
                    result += AddField("sessionid", fields[0]);
                }
                //Login UserCreate
                else if (option <= 6) {
                    result += AddField("username", fields[0]);
                    result += AddField("password", fields[1]);
                }
                //SendMove
                else if (option == 7) {
                    result += AddField("sessionid", fields[1]);
                    result += AddField("move", fields[1]);
                }
                else throw new ArgumentException("Invalid option!");
            }
            catch (ArgumentOutOfRangeException) {
                throw new Exception("Invalid number of parametrs");
            }

            return result;
        }

        private static string[,] GetArgArrayFromResponse(string response) {
            string[] args = response.Split("&&");
            int numOfArgs = args.Length;

            string[,] argArray = new string[numOfArgs, 2];

            for (int i = 0; i < numOfArgs; i++) {
                string[] arg = args[i].Split(":");
                argArray[i, 0] = arg[0];
                argArray[i, 1] = arg[1];
            }

            return argArray;
        }

        public struct LoginCommandResponse 
        {
            public readonly string sessionID;
            public readonly int result;
            public LoginCommandResponse(int result, string sessionID) {
                this.result = result;
                this.sessionID = sessionID;
            }
        }
        public static LoginCommandResponse LoginCommand(ref ServerConnection conn, string username, string password) {
            string command = CreateClientMessage(5, username, password);
            conn.SendMessage(command);
            string[,] argArray = GetArgArrayFromResponse(conn.ReadMessage());
            return new LoginCommandResponse(Int32.Parse(argArray[0, 1]), argArray[1, 1]);
        }
    }
}
