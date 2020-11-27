using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    class BaseModel
    {
        protected ServerConnection connection;
        public BaseModel(ServerConnection connection) {
            this.connection = connection;
        }
    }
}
