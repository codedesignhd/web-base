using System;
using System.Collections.Generic;
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
            List<int> codes = new List<int>();
            while (length > 0)
            {
                codes.Add(alphabet[_random.Next(0, alphabet.Length)]);
                length -= 1;
            }
            return string.Join("", codes);
        }
    }
}
