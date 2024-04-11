using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeDesign.Dtos
{
    public static class AppConsts
    {
        public const string DEFAULT_USER = "unknow";

    }

    public class RegexConst
    {
        public static Regex RegXUsername = new Regex("", RegexOptions.IgnoreCase, TimeSpan.FromSeconds(1.0));
    }



    public class ClaimTypesCustom
    {
        public const string Properties = "prp";
        public const string Fullname = "fna";
        public const string Username = "usr";
        //public static readonly string EMAIL = "emi";
    }


    public class FileExtension
    {
        public const string Doc = ".doc";
        public const string Docx = ".docx";
        public const string Png = ".png";
        public const string Jpg = ".jpg";
        public const string Jpeg = ".jpeg";
    }


    public class FileContentType
    {
        public const string Doc = "application/msword";
        public const string Docx = "application/vndoc";
        public const string Excel = "application/vndoc";
        public const string Pdf = "application/vndoc";
    }
}
