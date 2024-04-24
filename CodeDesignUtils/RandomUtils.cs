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
    }
}
