﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeDesign.Utilities
{

    public static class StringUtils
    {
        public static string TiengVietKhongDau(string str)
        {
            return str;
        }
        public static string ChuanHoa(this string str)
        {
            return Regex.Replace(str.Trim().ToLower(), "\\s+", "", RegexOptions.None);
        }
    }
}
