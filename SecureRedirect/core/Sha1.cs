using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Text;

namespace SecureRedirect.Core
{
    public class Sha1
    {
        public static string Compute(string str)
        {
            SHA1 sha = new SHA1CryptoServiceProvider();
            return BitConverter.ToString(sha.ComputeHash(UTF8Encoding.UTF8.GetBytes(str))).Replace("-", "");
        }
    }
}