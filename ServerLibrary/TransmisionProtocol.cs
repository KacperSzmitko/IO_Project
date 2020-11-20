using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLibrary
{
    class TransmisionProtocol
    {
        /// <summary>
        /// Function which formats client data by using our transmition protocol
        /// </summary>
        /// <param name="result">Reference to string where u want your result to be stored</param>
        /// <param name="option">*0 - Logout  1 - MatchHistory  2 - Rank  3 - SearchGame  4 - EndGame  5 - Login  6 - CreateUser  7 - SendMove</param>
        /// <param name="fields">*0-4 : SessionID    5-6 : Username Password   7 : SessionID Move</param>
        public static void CreateClientMessage(ref string result, int option,params string[] fields)
        {
            try
            {
                AddField("Option", option.ToString(), ref result);
                //Logout MatchHistory Rank SearchGame EndGame
                if (option >= 0 && option <= 4)
                {
                    AddField("SessionID", fields[0], ref result);
                }
                //Login UserCreate
                else if (option <= 6)
                {
                    AddField("Username", fields[0], ref result);
                    AddField("Password", fields[1], ref result);
                }
                //SendMove
                else if (option == 7)
                {
                    AddField("SessionID", fields[1], ref result);
                    AddField("Move", fields[1], ref result);
                }
                else throw new ArgumentException("Invalid option!");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Invalid number of parametrs");
            }
        }

        /// <summary>
        /// Function which formats client data by using our transmition protocol
        /// </summary>
        /// <param name="result">Reference to string where u want your result to be stored</param>
        /// <param name="succes">Information about client Request</param>
        /// <param name="option">0 - Default response without any fields  1 - Send data  2 - Information about found game  3 - Game actualization</param>
        /// <param name="fields">0 - Blank  1 - Data (XMLDocument)  2 - OpponentName OpponentRank  PlayerRank  3 - Score</param>
        public static void CreateServerMessage(ref string result,bool succes, int option, params string[] fields)
        {
            AddField("Response", succes.ToString(), ref result);
            try
            {
                if (option == 1)
                {
                    AddField("Data", fields[0], ref result);
                }
                else if (option == 2)
                {
                    AddField("OppName", fields[0], ref result);
                    AddField("OppRank", fields[1], ref result);
                    AddField("Rank", fields[2], ref result);
                }
                else if (option == 3)
                {
                    AddField("Score", fields[0], ref result);
                }
                else if (option != 0) throw new ArgumentException("Invalid option!");
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new Exception("Invalid number of parametrs");
            }

        }

        /// <summary>
        /// Function used to add single arguments to message
        /// </summary>
        /// <param name="fieldName">Name of filed which you want to add</param>
        /// <param name="value">Value of that field</param>
        /// <param name="result">Reference to your result message</param>
        private static void AddField(string fieldName,string value, ref string result)
        {
            result += fieldName + ":" + value + "$$";
        }

         

    }
}
