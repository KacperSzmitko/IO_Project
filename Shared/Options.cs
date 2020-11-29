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
        SEND_MATCH = 10 //                                                        Error:<>$$OppName:<>$$OppRank:<>$$PlayerRank:<>$$
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



