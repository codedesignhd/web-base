using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace CodeDesign.Models
{
    public abstract class AppConsts
    {
        public const string DEFAULT_USER = "unknow";
    }
    public abstract class ClaimTypesCustom
    {
        public const string THUOC_TINH = "prp";
        //public static readonly string FULL_NAME = "fna";
        public const string USERNAME = "usr";
        //public static readonly string EMAIL = "emi";
    }
}
