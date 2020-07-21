using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp.Security
{
    public class MD_5_Configuration
    {
        public static string Encrypted(string value)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(value);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {

            }
            return "";             
                    }
    }
}
