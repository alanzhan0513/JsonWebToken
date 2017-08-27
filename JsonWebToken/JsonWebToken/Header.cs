using System;
using System.Collections.Generic;
using System.Text;

namespace JsonWebToken
{
    public class Header
    {
        public Header(JwsHashAlgorithms alg, String typ) { this.alg = alg.ToString(); this.typ = typ; }

        public String alg { get; set; }

        public String typ { get; set; }
    }
}
