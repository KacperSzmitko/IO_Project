using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public abstract class LoggedModel : BaseModel
    {
        protected User user;
        public User User { get { return user; } set { user = value; } }

        public LoggedModel(ServerConnection connection, User user) : base(connection) {
            this.user = user;
        }
    }
}
