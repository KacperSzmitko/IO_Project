using System;
namespace Shared
{
    public enum Options
    {
        //                          CLIENT_FIELDS                                 SERVER RESPONSE
        LOGOUT = 0, //Ready         SessionID:<>$$                                Error:<>$$
        MATCH_HISTORY = 1, //Ready  SessionID:<>$$                                Error:<>$$DATA:<xmlDoc>$$
        RANK = 2, //Ready           SessionID:<>$$                                Error:<>$$DATA:<xmlDoc>$$
        SEARCH_GAME = 3, //Ready    SessionID:<>$$                                Error:<>$$
        END_GAME = 4,//             SessionID:<>$$                                Error:<>$$
        LOGIN = 5, //Ready          Username:<>$$Password:<>$$                    Error:<>$$SessionID:<>$$Elo:<>$$
        CREATE_USER = 6, //Ready    Username:<>$$Password:<>$$                    Error:<>$$
        SEND_MOVE = 7, //           SessionID:<>$$Move:<>$$                       Error:<>$$Score:np.1-0$$ gdzie pierwsza jest klienta ktroy wyslal ruch
        DISCONNECT = 8, //Ready                                                   Error:<>$$
        CHECK_USER_NAME = 9, //     Username:<>$$                                 Error:<>$$
        SEND_MATCH = 10 //                                                        Error:<>$$OppName:<>$$OppRank:<>$$IsCross:<>

    }


}



