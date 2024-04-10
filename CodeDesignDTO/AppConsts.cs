using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeDesign.Dtos
{
    public static class AppConsts
    {
        public const string DEFAULT_USER = "unknow";
        public static Regex RegXUsername = new Regex("", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1.0));
    }
    public class ClaimTypesCustom
    {
        public const string Properties = "prp";
        public static readonly string Fullname = "fna";
        public const string Username = "usr";
        //public static readonly string EMAIL = "emi";
    }
}
