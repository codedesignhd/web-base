using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeDesignUtilities.Constants
{
    public abstract class RegexConst
    {
        private static TimeSpan _defaultTimeSpan = TimeSpan.FromSeconds(1.0);
        public static readonly Regex RegXUsername = new Regex("", RegexOptions.IgnoreCase, _defaultTimeSpan);
    }
}
