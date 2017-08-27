using System;
using System.Collections.Generic;
using System.Text;

namespace JsonWebToken
{
    public class Payload
    {
        public Payload()
        {
            DateTime utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime dtIssueTime = DateTime.Now;

            this.iat = (Int32)dtIssueTime.Subtract(utc0).TotalSeconds;
            // You can set expiration time of token.
            this.exp = (Int32)dtIssueTime.AddMinutes(60).Subtract(utc0).TotalSeconds;
        }

        public Payload(DateTime dtExp)
        {
            DateTime utc0 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            this.iat = (Int32)DateTime.Now.Subtract(utc0).TotalSeconds;
            // You can set expiration time of token.
            this.exp = (Int32)dtExp.Subtract(utc0).TotalSeconds;
        }

        /// <summary> issuer
        /// 
        /// </summary>
        public String iss { get; set; }

        /// <summary> subject
        /// 
        /// </summary>
        public String sub { get; set; }

        /// <summary> audience
        /// 
        /// </summary>
        public String aud { get; set; }

        /// <summary> expiration time
        /// 
        /// </summary>
        public Int32 exp { get; set; }

        /// <summary> issued at
        /// 
        /// </summary>
        public Int32 iat { get; set; }
    }
}
