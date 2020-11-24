using System;
using System.Collections.Generic;
using System.Text;

namespace Client 
{
    public class User 
    {
        private readonly string sessionID;
        private readonly string userID;
        private readonly string userName;

        public string SessionID { get { return sessionID; } }
        public string UserID { get { return userID; } }
        public string UserName { get { return userName; } }

        public User(string sessionID, string userID, string userName) {
            this.sessionID = sessionID;
            this.userID = userID;
            this.userName = userName;
        }

    }
}
