using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Host.Core.Common
{
    public class Security
    {
        public static string GetHash(string Password, HashType hashType)
        {
            string hashString;
            switch (hashType)
            {
                case HashType.MD5:
                    hashString = GetMD5(Password);
                    break;
                case HashType.SHA1:
                    hashString = GetSHA1(Password);
                    break;
                case HashType.SHA256:
                    hashString = GetSHA256(Password);
                    break;
                case HashType.SHA512:
                    hashString = GetSHA512(Password);
                    break;
                default:
                    hashString = "Tipo de encriptacion incorrecto";
                    break;
            }
            return hashString;
        }

        private static string GetMD5(string text)
        {
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            MD5 hashString = new MD5CryptoServiceProvider();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return GetBase64(hex);
        }

        private static string GetSHA1(string text)
        {
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return GetBase64(hex);
        }

        private static string GetSHA256(string text)
        {
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return GetBase64(hex);

        }

        private static string GetSHA512(string text)
        {
            ASCIIEncoding UE = new ASCIIEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA512Managed hashString = new SHA512Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return GetBase64(hex);
        }

        private static string GetBase64(string _HashValue)
        {
            _HashValue = _HashValue.Replace("-", "");

            byte[] resultantArray = new byte[_HashValue.Length / 2];
            for (int i = 0; i < resultantArray.Length; i++)
            {
                resultantArray[i] = Convert.ToByte(_HashValue.Substring(i * 2, 2), 16);
            }
            byte[] data = resultantArray;
            string base64 = Convert.ToBase64String(data);

            return base64;
        }

        public static string Encryption(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            string base64 = Convert.ToBase64String(bytes);
            return base64;
        }

        public static string Decoding(string value)
        {
            byte[] bytes = Convert.FromBase64String(value);
            string Texto = Encoding.UTF8.GetString(bytes);
            return Texto;
        }


    }



    public enum HashType : int
    {
        MD5,
        SHA1,
        SHA256,
        SHA512
    }
}
