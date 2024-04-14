using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace CodeDesign.Utilities
{
    public static class CryptoUtils
    {
        private const string DefaultKey = "";
        private static readonly string DefaultSalt = "DefaultSalt";
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

        public static string HashPasword(string password, string salt = null)
        {
            if (!string.IsNullOrWhiteSpace(salt))
            {
                return HmacSHA256(DefaultKey, password + salt);
            }
            return HmacSHA256(DefaultKey, password + DefaultSalt);
        }
    }
}
