using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CodeDesignUtilities
{
    public static class RandomUtils
    {

        public static Random _random;
        public static Random Rand
        {
            get
            {
                if (_random is null)
                {
                    _random = new Random();
                }
                return _random;
            }
        }

        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        private static readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        private static readonly char[] _validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
        public static string GenCode(int length)
        {
            List<int> codes = new List<int>();
            while (length > 0)
            {
                codes.Add(Rand.Next(0, 10));
                length -= 1;
            }
            return string.Join("", codes);
        }


        public static string Generate(string alphabet = StringUtils.Alphabet, int length = 21)
        {
            if (string.IsNullOrWhiteSpace(alphabet))
            {
                return string.Empty;
            }
            List<char> codes = new List<char>();
            while (length > 0)
            {
                codes.Add(alphabet[Rand.Next(0, alphabet.Length)]);
                length -= 1;
            }
            return string.Join("", codes);
        }

        public static string GenerateId(int length = 22)
        {
            // Get the current timestamp in milliseconds since the Unix epoch
            double timestamp = (DateTime.UtcNow - _epoch).TotalMilliseconds;

            // Generate a random 8-byte array
            byte[] randomBytes = new byte[8];
            _rng.GetBytes(randomBytes);

            // Combine the timestamp and random bytes into a byte array
            byte[] combinedBytes = new byte[12];
            Array.Copy(BitConverter.GetBytes(timestamp), 0, combinedBytes, 0, 8);
            Array.Copy(randomBytes, 0, combinedBytes, 8, 4);

            // Convert the combined bytes to a hexadecimal string
            string hexString = BitConverter.ToString(combinedBytes).Replace("-", string.Empty);

            // Convert the hexadecimal string to a base-64 string using a custom character set
            string base64String = Convert.ToBase64String(hexString.ToCharArray().Select(c => (byte)c).ToArray());

            // Extract the desired number of characters from the base-64 string
            return base64String.Substring(0, length);
        }
    }
}
