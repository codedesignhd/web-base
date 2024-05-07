using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CodeDesign.Utilities
{
    public abstract class CryptoUtils
    {
        private const string DefaultKey = "CodeDesign";
        private const string DefaultSalt = "DefaultSalt";

        #region Hmac
        public static string HmacSHA256(string key, string text)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (var sha = new HMACSHA256(keyBytes))
            {
                StringBuilder hash = new StringBuilder();
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] crypto = sha.ComputeHash(bytes);
                foreach (byte b in crypto)
                {
                    hash.Append(b.ToString("x2"));
                }
                return hash.ToString();
            }
        }
        public static string HmacSHA512(string key, string text)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            using (var sha = new HMACSHA512(keyBytes))
            {
                StringBuilder hash = new StringBuilder();
                byte[] bytes = Encoding.UTF8.GetBytes(text);
                byte[] crypto = sha.ComputeHash(bytes);
                foreach (byte b in crypto)
                {
                    hash.Append(b.ToString("x2"));
                }
                return hash.ToString();
            }
        }
        #endregion

        #region Md5
        public static string GetHexMD5(string input, bool isUtf8 = true)
        {
            StringBuilder sb = new StringBuilder();
            using (var md5 = MD5.Create())
            {
                byte[] plainTextBytes = isUtf8 ? Encoding.UTF8.GetBytes(input) : Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(plainTextBytes);
                for (int index = 0; index < hash.Length; ++index)
                    sb.Append(hash[index].ToString("x2"));
            }
            return sb.ToString();
        }

        public static byte[] GetHashMd5(string input, bool isUtf8 = true)
        {
            using (var md5 = MD5.Create())
            {
                byte[] plainTextBytes = isUtf8 ? Encoding.UTF8.GetBytes(input) : Encoding.ASCII.GetBytes(input);
                return md5.ComputeHash(plainTextBytes);
            }
        }
        #endregion
        public static string HashPasword(string password, string salt = null)
        {
            if (!string.IsNullOrWhiteSpace(salt))
            {
                return HmacSHA256(DefaultKey, password + salt);
            }
            return HmacSHA256(DefaultKey, password + DefaultSalt);
        }
        public static string Base64Encode(string plainText)
        {
            byte[] plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Encode(string plainText)
        {
            return plainText;
        }

        public static string Decode(string token)
        {
            return token;
        }
    }
}
