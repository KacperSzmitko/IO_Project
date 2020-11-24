using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ServerLibrary
{
    //Class which provides static security methods
    abstract class Security
    {

        //Function used to generate self-snged X509 Certificate
        public static void MakeCert()
        {
            var ecdsa = ECDsa.Create(); // generate asymmetric key pair
            var req = new CertificateRequest("cn=foobar", ecdsa, HashAlgorithmName.SHA256);
            var cert = req.CreateSelfSigned(DateTimeOffset.Now, DateTimeOffset.Now.AddYears(5));

            // Create PFX (PKCS #12) with private key
            File.WriteAllBytes("./cert.pfx", cert.Export(X509ContentType.Pfx, "P@55w0rd"));
            // Create Base 64 encoded CER (public key only)
            File.WriteAllText("./cert.cer",
                "-----BEGIN CERTIFICATE-----\r\n"
                + Convert.ToBase64String(cert.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks)
                + "\r\n-----END CERTIFICATE-----");

        }

        public static string HashPassword(string passwd)
        {
            int saltSize = 16;
            int totalSize = saltSize + passwd.Length;
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[saltSize]);

            var pbkdf2 = new Rfc2898DeriveBytes(passwd, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(passwd.Length);

            byte[] hashBytes = new byte[totalSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, passwd.Length);
            return Convert.ToBase64String(hashBytes);
        }

        public static bool VerifyPassword(string passwdHash,string passwd)
        {
            byte[] hashBytes = Convert.FromBase64String(passwdHash);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(passwd, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(passwd.Length);
            /* Compare the results */
            for (int i = 0; i < passwd.Length; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }
    }

}
