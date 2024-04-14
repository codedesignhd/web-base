using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeDesign.Utilities.Constants
{
    public class RegexConst
    {
        public static Regex RegXUsername = new Regex("", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1.0));
    }
}
