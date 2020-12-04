using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ServerLibrary
{
    //Class which provides static security methods
    public interface Security
    {
        public string HashPassword(string passwd);

        public bool VerifyPassword(string passwdHash, string passwd);

    }

}
