using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    public enum CellStatus
    {
        EMPTY,
        USER_O,
        USER_X,
        OPPONENT_O,
        OPPONENT_X
    }

    public enum MoveResult
    {
        ROUND_NOT_OVER,
        USER_WON,
        USER_LOST
    }
}
