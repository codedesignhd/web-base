using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.BL.Providers
{
    public static class CouchbaseKeyProvider
    {
        public static string GenResetPasswordKey(string username)
        {
            return string.Format("reset_pwd_{0}", username);
        }
    }
}
