using System;
using System.Collections.Generic;
using System.Text;

namespace JsonWebToken
{
    public static class Base64Url
    {
        public static String Encode(Byte[] btInput)
        {
            return Convert.ToBase64String(btInput).Split(new char[]
            {
                '='
            })[0].Replace('+', '-').Replace('/', '_');
        }

        public static Byte[] Decode(String strInput)
        {
            String strOutput = strInput.Replace('-', '+');
            strOutput = strOutput.Replace('_', '/');
            switch (strOutput.Length % 4)
            {
                case 0: break;
                case 2: strOutput += "=="; break;
                case 3: strOutput += "="; break;
                default: throw new ArgumentOutOfRangeException("input", "Illegal base64url string!");
            }
            return Convert.FromBase64String(strOutput);
        }
    }
}
