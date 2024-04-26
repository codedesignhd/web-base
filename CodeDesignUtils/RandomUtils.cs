using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Utilities
{
    public abstract class RandomUtils
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
                codes.Add(_random.Next(0, 10));
                length -= 1;
            }
            return string.Join("", codes);
        }

    }
}
