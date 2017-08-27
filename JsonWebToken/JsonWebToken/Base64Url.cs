using System;
using System.Collections.Generic;
using System.Text;

namespace JsonWebToken
{
    public static class Base64Url
    {
        public static String Encode(Byte[] input)
        {
            return Convert.ToBase64String(input).Split(new char[]
            {
                '='
            })[0].Replace('+', '-').Replace('/', '_');
        }

        public static Byte[] Decode(String input)
        {
            String output = input.Replace('-', '+');
            output = output.Replace('_', '/');
            switch (output.Length % 4)
            {
                case 0: break;
                case 2: output += "=="; break;
                case 3: output += "="; break;
                default: throw new ArgumentOutOfRangeException("input", "Illegal base64url string!");
            }
            return Convert.FromBase64String(output);
        }
    }
}
