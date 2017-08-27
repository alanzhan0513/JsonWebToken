using JsonWebToken;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            String strKey = "ShuaiZhan";

            // Encode
            Payload payload = new Payload() { iss = "rsesst60313@gmail.com", aud = "Hello", sub = "WORD" };
            String strToken = JsonWebToken.JsonWebToken.Encode(payload, strKey, JwsHashAlgorithms.RS256);
            Console.WriteLine(strToken);

            // Decode
            String payloadData = JsonWebToken.JsonWebToken.Decode(strToken, strKey, true);
            Console.WriteLine(payloadData);

            // Verify true
            Boolean b1 = JsonWebToken.JsonWebToken.Verify(strToken, strKey);
            Console.WriteLine(b1 ? "success" : "failure");

            // Verify false
            Boolean b2 = JsonWebToken.JsonWebToken.Verify(strToken + "1234", strKey);
            Console.WriteLine(b2 ? "success" : "failure");

            Console.Read();
        }
    }
}
