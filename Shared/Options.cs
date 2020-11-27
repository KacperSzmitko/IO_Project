using System;
namespace Shared
{
    public enum Options
    {
        //                          CLIENT_FIELDS                                 SERVER RESPONSE
        LOGOUT = 0, //Ready         SessionID:<>$$                                Result:<>$$
        MATCH_HISTORY = 1, //Ready  SessionID:<>$$                                Result:<>$$DATA:<xmlDoc>$$
        RANK = 2, //Ready           SessionID:<>$$                                Result:<>$$DATA:<xmlDoc>$$
        SEARCH_GAME = 3, //Ready    SessionID:<>$$                                Result:<>$$OppName:<>$$OppRank:<>$$PlayerRank:<>$$
        END_GAME = 4,//             SessionID:<>$$                                Result:<>$$
        LOGIN = 5, //Ready          SessionID:<>$$Username:<>$$Password:<>$$      Result:<>$$SessionID:<>$$Elo:<>$$
        CREATE_USER = 6, //Ready    SessionID:<>$$Username:<>$$Password:<>$$      Result:<>$$
        SEND_MOVE = 7, //           SessionID:<>$$Move:<>$$                       Result:<>$$Score:np.1-0$$ gdzie pierwsza jest klienta ktroy wyslal ruch
        DISCONNECT = 8, //Ready                                                   Result:<>$$
        CHECK_USER_NAME = 9 //      Username:<>$$                                 Result:<>$$

    }

}



