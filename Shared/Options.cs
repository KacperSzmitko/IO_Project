using System;
namespace Shared
{
    public enum Options
    {
        //                          CLIENT_FIELDS                                   SERVER RESPONSE
        LOGOUT = 0, //Ready         $$SessionID:<>$$                                $$Response:<>$$
        MATCH_HISTORY = 1, //Ready  $$SessionID:<>$$                                $$Response:<>$$DATA:<xmlDoc>$$
        RANK = 2, //Ready           $$SessionID:<>$$                                $$Response:<>$$DATA:<xmlDoc>$$
        SEARCH_GAME = 3, //Ready    $$SessionID:<>$$                                $$Response:<>$$OppName:<>$$OppRank:<>$$PlayerRank:<>$$
        END_GAME = 4,//             $$SessionID:<>$$                                $$Response:<>$$
        LOGIN = 5, //Ready          $$SessionID:<>$$Username:<>$$Password:<>$$      $$Response:<>$$SessionID:<>$$Elo:<>$$
        CREATE_USER = 6, //Ready    $$SessionID:<>$$Username:<>$$Password:<>$$      $$Response:<>$$
        SEND_MOVE = 7, //           $$SessionID:<>$$Move:<>$$                       $$Response:<>$$Score:np.1-0$$ gdzie pierwsza jest klienta ktroy wyslal ruch
        DISCONNECT = 8 //Ready      $$                                              $$Response:<>$$
    }

}



