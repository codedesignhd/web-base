using System;
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

        public static string HideEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return string.Empty;
            int visibleLength = 3;

            return email.Length > visibleLength ? string.Format("{0}{1}", email.Substring(0, visibleLength), new string('*', email.Length - visibleLength)) : string.Empty;
        }
    }
}
