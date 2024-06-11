using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignUtilities.Constants
{
    public abstract class FileExtension
    {
        public const string Doc = ".doc";
        public const string Docx = ".docx";
        public const string Png = ".png";
        public const string Jpg = ".jpg";
        public const string Jpeg = ".jpeg";
        public const string Emf = ".emf";

        //public static readonly List<string> ValidImageExt = new List<string> { Png, Jpg, Jpeg, Emf };
        public static readonly string[] ValidImageExt = new string[] { Png, Jpg, Jpeg, Emf };
    }
}
