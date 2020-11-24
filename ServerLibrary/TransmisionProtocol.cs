using System;
using System.Collections.Generic;
using System.Text;

namespace ServerLibrary
{
    class TransmisionProtocol
    {

        /// <summary>
        /// Function which formats server data by using our transmition protocol
        /// </summary>
        /// <param name="result">Reference to string where u want your result to be stored</param>
        /// <param name="succes">Information about client Request</param>
        /// <param name="option">0 - Logout  1 - MatchHistory  2 - Rank  3 - SearchGame  4 - EndGame  5 - Login  6 - CreateUser  7 - SendMove 8 - Disconnect </param>  
        /// <param name="fields">0,4,6 - Blank  1,2 - Data (XMLDocument)  3 - OpponentName OpponentRank  PlayerRank  5 - SessionID Elo  7 - Score  </param>  
        public static string CreateServerMessage(bool succes, int option, params string[] fields)
        {
            string result = "";
            AddField("Response", succes.ToString(), ref result);
            // We got it! Prepare answer from given data
            if (succes)
            {
                try
                {
                    // Options can by only in range of <0,7>
                    if (option > 7 || option < 0) throw new ArgumentException("Invalid option!");
                    //GetMatchHisotry and GetRank
                    if (option == 1 || option == 2)
                    {
                        AddField("Data", fields[0], ref result);
                    }
                    //SearchGame
                    else if (option == 3 && fields.Length >0)
                    {
                        AddField("OppName", fields[0], ref result);
                        AddField("OppRank", fields[1], ref result);
                        AddField("Rank", fields[2], ref result);
                    }
                    //Login
                    else if (option == 5)
                    {
                        AddField("SessionID", fields[0], ref result);
                        AddField("Rank", fields[1], ref result);
                    }
                    //SendMove
                    else if (option == 7)
                    {
                        AddField("Score", fields[0], ref result);
                    }

                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new Exception("Invalid number of parametrs");
                }
            }
            //Tell client what went wrong
            else
            {
                AddField("Error", fields[0], ref result);
            }
            return result;

        }

        /// <summary>
        /// Function used to add single arguments to message
        /// </summary>
        /// <param name="fieldName">Name of filed which you want to add</param>
        /// <param name="value">Value of that field</param>
        /// <param name="result">Reference to your result message</param>
        public static void AddField(string fieldName,string value, ref string result)
        {
            result += fieldName + ":" + value + "$$";
        }

         

    }
}
