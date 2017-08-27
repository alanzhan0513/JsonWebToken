using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace JsonWebToken
{
    public enum JwsHashAlgorithms
    {
        RS256 = 0,
        HS384 = 1,
        HS512 = 2
    }

    public class JsonWebToken
    {
        static Dictionary<JwsHashAlgorithms, Func<Byte[], Byte[], Byte[]>> dicHashAlgorithms;

        static JsonWebToken()
        {
            dicHashAlgorithms = new Dictionary<JwsHashAlgorithms, Func<Byte[], Byte[], Byte[]>>
            {
                { JwsHashAlgorithms.RS256, (key, value) => { using (HMACSHA256 SHA256 = new HMACSHA256(key)) { return SHA256.ComputeHash(value); } } },
                { JwsHashAlgorithms.HS384, (key, value) => { using (HMACSHA384 SHA384 = new HMACSHA384(key)) { return SHA384.ComputeHash(value); } } },
                { JwsHashAlgorithms.HS512, (key, value) => { using (HMACSHA512 SHA512 = new HMACSHA512(key)) { return SHA512.ComputeHash(value); } } }
            };
        }

        public static String Encode<T>(T payload, String strKey, JwsHashAlgorithms algorithm) where T : class 
        {
            return Encode(payload, Encoding.UTF8.GetBytes(strKey), algorithm);
        }

        public static String Encode<T>(T payload, Byte[] btKey, JwsHashAlgorithms algorithm) where T : class
        {
            Header header = new Header(algorithm, "JWT");

            Byte[] btHeader = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            Byte[] btPayload = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload, Formatting.None));

            String strToSign = Base64Url.Encode(btHeader) + "." + Base64Url.Encode(btPayload);
            Byte[] btToSign = Encoding.UTF8.GetBytes(strToSign);
            Byte[] btSignATure = dicHashAlgorithms[algorithm](btKey, btToSign);

            return strToSign + "." + Base64Url.Encode(btSignATure);
        }

        public static String Decode(String strToken, String strKey, Boolean bVerify = false)
        {
            String[] parts = strToken.Split('.');
            String strHeader = parts[0];
            String strPayload = parts[1];
            Byte[] crypto = Base64Url.Decode(parts[2]);

            String strJsonHeader = Encoding.UTF8.GetString(Base64Url.Decode(strHeader));
            JObject headerData = JObject.Parse(strJsonHeader);
            String strJsonPayload = Encoding.UTF8.GetString(Base64Url.Decode(strPayload));
            JObject payloadData = JObject.Parse(strJsonPayload);

            if (bVerify)
            {
                Byte[] btSign = Encoding.UTF8.GetBytes(String.Concat(strHeader, ".", strPayload));
                Byte[] btKey = Encoding.UTF8.GetBytes(strKey);
                String strAlg = (String)headerData["alg"];

                Byte[] strSignATure = dicHashAlgorithms[GetHashAlgorithm(strAlg)](btKey, btSign);
                String decodedCrypto = Convert.ToBase64String(crypto);
                String decodedSignature = Convert.ToBase64String(strSignATure);

                if (decodedCrypto != decodedSignature)
                {
                    throw new ApplicationException(String.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
                }
            }

            return payloadData.ToString();
        }

        public static Boolean Verify(String strToken, String strKey)
        {
            try
            {
                Decode(strToken, strKey, true);
                return true;
            }
            catch
            {
                return false;
            }
        }

        static JwsHashAlgorithms GetHashAlgorithm(String algorithm)
        {
            switch (algorithm)
            {
                case "RS256": return JwsHashAlgorithms.RS256;
                case "HS384": return JwsHashAlgorithms.HS384;
                case "HS512": return JwsHashAlgorithms.HS512;
                default: throw new InvalidOperationException("Algorithm not supported.");
            }
        }
    }
}
