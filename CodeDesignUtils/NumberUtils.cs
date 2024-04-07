using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
    public class NumberUtils
    {
        public static int RoundToInt(double value)
        {
            double ceiling = Math.Ceiling(value);
            if (ceiling - value >= 0.499999)
            {
                return (int)Math.Floor(value);
            }
            return (int)Math.Ceiling(value);
        }
        
    }

    public static class NumberExt
    {
        
    }
}
