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
    }
}
