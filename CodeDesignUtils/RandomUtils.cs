using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Utilities
{
    public abstract class RandomUtils
    {
        private static Random _rand;
        public static Random Rand
        {
            get
            {
                if (_rand is null)
                {
                    _rand = new Random();
                }
                return _rand;
            }
        }
    }
}
