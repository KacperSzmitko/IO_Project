using System;
namespace Shared
{
    public enum Options
    {
        //                          CLIENT_FIELDS                                 SERVER RESPONSE
        LOGOUT = 0, //Ready         SessionID:<>$$                                Result:<>$$
        MATCH_HISTORY = 1, //Ready  SessionID:<>$$                                Result:<>$$DATA:<xmlDoc>$$
        RANK = 2, //Ready           SessionID:<>$$                                Result:<>$$DATA:<xmlDoc>$$
        SEARCH_GAME = 3, //Ready    SessionID:<>$$                                Result:<>$$
        END_GAME = 4,//             SessionID:<>$$                                Result:<>$$
        LOGIN = 5, //Ready          SessionID:<>$$Username:<>$$Password:<>$$      Result:<>$$SessionID:<>$$Elo:<>$$
        CREATE_USER = 6, //Ready    SessionID:<>$$Username:<>$$Password:<>$$      Result:<>$$
        SEND_MOVE = 7, //           SessionID:<>$$Move:<>$$                       Result:<>$$Score:np.1-0$$ gdzie pierwsza jest klienta ktroy wyslal ruch
        DISCONNECT = 8, //Ready                                                   Result:<>$$
        CHECK_USER_NAME = 9, //     Username:<>$$                                 Result:<>$$
        SEND_MATCH = 10 //                                                        Result:<>$$OppName:<>$$OppRank:<>$$PlayerRank:<>$$
    }

    public enum ErrorCodes
    {
        NO_ERROR = 0,
        NOT_LOGGED_IN = 1,
        USER_NOT_FOUND = 2,
        DB_CONNECTION_ERROR = 3,
        GAME_IS_ALREADY_SEARCHED = 4,
        USER_ALREADY_LOGGED_IN = 5,
        INCORRECT_PASSWORD = 6,
        USER_ALREADY_EXISTS = 7,
        NOT_IN_GAME = 8,
        MOVE_NOT_ALLOWED = 9,
    }

}



