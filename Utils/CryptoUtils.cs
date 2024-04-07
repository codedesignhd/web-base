using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Utils
{
    public class CryptoUtils
    {
        private static readonly string SALT = "DefaultSalt";
        public static string HashSHA256(string text)
        {
            using (var sha = new HMACSHA256())
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

        public static string HashPasword(string password, string salt=null)
        {
            if (!string.IsNullOrWhiteSpace(salt))
            {
                return HashSHA256(password + salt);
            }
            return HashSHA256(password + SALT);
        }
    }
}
