using System;
namespace Shared
{
    public enum Options
    {
                              //    ADDITIONAl CLIENT FIELDS                      SERVER RESPONSE
        LOGOUT = 0,           //    sessionid:<>$$                                Error:<>$$
        MATCH_HISTORY = 1,    //    sessionid:<>$$                                Error:<>$$DATA:<xmlDoc>$$
        RANK = 2,             //    sessionid:<>$$                                Error:<>$$DATA:<xmlDoc>$$
        SEARCH_GAME = 3,      //    sessionid:<>$$                                Error:<>$$
        END_GAME = 4,         //    sessionid:<>$$                                Error:<>$$PlayerElo:<>$$OppElo:<>$$ 
        LOGIN = 5,            //    username:<>$$password:<>$$                    Error:<>$$SessionID:<>$$Elo:<>$$
        CREATE_USER = 6,      //    username:<>$$password:<>$$                    Error:<>$$
        SEND_MOVE = 7,        //    sessionid:<>$$move:<>$$                       Error:<>$$Score:<>$$ np. 1-0 gdzie pierwsza jest klienta ktroy wyslal ruch
        DISCONNECT = 8,       //                                                  Error:<>$$
        CHECK_USER_NAME = 9,  //    username:<>$$                                 Error:<>$$
        SEND_MATCH = 10,      //                                                  Error:<>$$OppName:<>$$OppRank:<>$$IsCross:<>
        OPP_MOVE = 11,        //                                                  Error:<>$$Score:<>$$OppMove:<>$$
        STOP_SEARCHING = 12,  //                                                  Error:<>$$    
        
    }


}



