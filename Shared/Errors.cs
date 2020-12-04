using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{   public enum ErrorCodes
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
