using NPOI.HPSF;
using NPOI.POIFS.Crypt;
using NPOI.Util;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChatApp.Security
{
    public class AES
    {
        private static SecretKeySpec secretKey;
        private static byte[] key;

       
        public static void setKey(String myKey)
        {
            
            try
            {
                Encoding ascii = Encoding.ASCII;
                Encoding unicode = Encoding.Unicode;
                key = unicode.GetBytes(myKey);
                SHA1 sha = SHA1Managed.Create();
                key = sha.ComputeHash(key);
                key = Arrays.CopyOf(key, 16);
                secretKey = new SecretKeySpec(key, "AES");
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }


        public static String encrypt(String strToEncrypt, String secret)
        {
            try
            {
                Encoding ascii = Encoding.ASCII;
                Encoding unicode = Encoding.Unicode;

                setKey(secret);
                Cipher cipher = Cipher.GetInstance("AES/ECB/PKCS5Padding");
                cipher.Init(Cipher.ENCRYPT_MODE, secretKey);
                return Base64.Encode(cipher.DoFinal(unicode.GetBytes(strToEncrypt))).ToString();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static String decrypt(String strToDecrypt, String secret)
        {
            try
            {
                setKey(secret);
                Cipher cipher = Cipher.GetInstance("AES/ECB/PKCS5PADDING");
                cipher.Init(Cipher.DECRYPT_MODE, secretKey);
                var value = cipher.DoFinal(Base64.Decode(strToDecrypt)).ToString();
                return value;
            }
            catch (Exception e)
            {
                throw e;
            }
           
        }
    }
}
