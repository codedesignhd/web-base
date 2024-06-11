using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeDesignUtilities
{
    public static class StringUtils
    {
        public static string TiengVietKhongDau(string input)
        {
            string[] strArray1 = new string[8];
            string[] strArray2 = new string[8]
            { "a","e","o","u","i","y","d"," " };
            strArray1[0] = "àảáạãâầấẩậẫăằẳắặẵ";
            strArray1[1] = "ềếệễểêẹéèẻẽ";
            strArray1[2] = "òỏóọõôồổốộỗơớợởờỡ";
            strArray1[3] = "úùủụừứựửữưũ";
            strArray1[4] = "ịỉìĩí";
            strArray1[5] = "ỷýỵỳỹ";
            strArray1[6] = "đ";
            strArray1[7] = "^a-z0-9";
            for (int index = 0; index < 8; ++index)
                input = Regex.Replace(input, "[" + strArray1[index] + "]", strArray2[index], RegexOptions.IgnoreCase);
            return input;
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
        public static string GenSKU(string input)
        {
            input = TiengVietKhongDau(input);
            return Regex.Replace(string.Join("-", input.ToLower().Split(' ')), "(-)\\1{1,}", "$1");
        }
    }
}
