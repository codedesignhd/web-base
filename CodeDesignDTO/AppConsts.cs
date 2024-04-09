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
        public const string THUOC_TINH = "prp";
        //public static readonly string FULL_NAME = "fna";
        public const string USERNAME = "usr";
        //public static readonly string EMAIL = "emi";
    }
}
