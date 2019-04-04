using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using Jose;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Utilities.Encoders;
using Org.BouncyCastle.Security;

namespace jose_jwt_test
{
    class Program
    {
       
        static void Main(string[] args)
        {

            JwtPayload payload = new JwtPayload();
            Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            payload.Add("aud", "https://iam.api.cloud.yandex.net/iam/v1/tokens");
            payload.Add("iss", "%iss%");
            payload.Add("iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            payload.Add("exp", DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 3600);

            var reader = File.OpenText("C:\\Users\\User\\source\\repos\\ConsoleApp1\\ConsoleApp1\\private.key");
            PemReader pRd = new PemReader(reader);
            RsaPrivateCrtKeyParameters pKey = (RsaPrivateCrtKeyParameters)pRd.ReadObject();
            pRd.Reader.Close();

            var rsa= RSA.Create();
            
            rsa.ImportParameters(DotNetUtilities.ToRSAParameters(pKey));

            IDictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("kid", "%kid%");
            dict.Add("typ", "JWT");

            string token = Jose.JWT.Encode(payload.SerializeToJson(), rsa, JwsAlgorithm.PS256, dict);
            Console.WriteLine(token);
        }
    }
}