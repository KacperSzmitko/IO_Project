using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class ServerCommands
    {
        private static string CreateCommandFromArgArray(string[,] argArray) {
            string command = "";
            for (int i = 0; i < argArray.GetLength(0); i++) {
                command += argArray[i, 0] + "=" + argArray[i, 1] + ";";
            }
            return command;
        }

        private static string[,] GetArgArrayFromResponse(string response) {
            string[] args = response.Split(";");
            int numOfArgs = args.Length;

            string[,] argArray = new string[numOfArgs, 2];

            for (int i = 0; i < numOfArgs; i++) {
                string[] arg = args[i].Split("=");
                argArray[i, 0] = arg[0];
                argArray[i, 1] = arg[1];
            }

            return argArray;
        }

        public struct LoginCommandResponse 
        { 
            public readonly string sessionID, userID, userName;
            public LoginCommandResponse(string sessionID, string userID, string userName) {
                this.sessionID = sessionID;
                this.userID = userID;
                this.userName = userName;
            }

        }
        public static LoginCommandResponse LoginCommand(ServerConnection conn, string userName, string password) {
            string command = CreateCommandFromArgArray(new string[,] { { "username", userName }, { "password", password } });
            conn.SendMessage(command);
            string[,] argArray = GetArgArrayFromResponse(conn.ReadMessage());
            return new LoginCommandResponse(argArray[0, 1], argArray[1, 1], argArray[2, 1]);
        }
    }
}
