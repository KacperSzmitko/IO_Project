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
        /// <param name="succes">Information about client Request</param>
        /// <param name="option">0 - Logout  1 - MatchHistory  2 - Rank  3 - SearchGame  4 - EndGame  5 - Login  6 - CreateUser  7 - SendMove </param>  
        /// <param name="fields">0,4,6 - Blank  1,2 - Data (XMLDocument)  3 - OpponentName OpponentRank  PlayerRank  5 - SessionID  7 - Score  </param>  
        public static void CreateServerMessage(ref string result,bool succes, int option, params string[] fields)
        {
            AddField("Response", succes.ToString(), ref result);
            try
            {
                if (option > 7 || option < 0) throw new ArgumentException("Invalid option!");
                if (option == 1 || option == 2)
                {
                    AddField("Data", fields[0], ref result);
                }
                else if (option == 3)
                {
                    AddField("OppName", fields[0], ref result);
                    AddField("OppRank", fields[1], ref result);
                    AddField("Rank", fields[2], ref result);
                }
                else if (option == 5)
                {
                    AddField("SessionID", fields[0], ref result);
                }
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
