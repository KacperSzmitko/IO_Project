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
        /// <param name="option">0 - Default response without any fields  1 - Send data  2 - Information about found game  3 - Game actualization  4 - Send Session ID </param>  
        /// <param name="fields">0 - Blank  1 - Data (XMLDocument)  2 - OpponentName OpponentRank  PlayerRank  3 - Score  4 - SessionID</param>  
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
                else if (option == 4)
                {
                    AddField("SessionID", fields[0], ref result);
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
