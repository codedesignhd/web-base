using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CodeDesign.Utilities
{
    public class CommonUtils
    {
        public static string GetIpAddress(HttpContext context)
        {
            string ipAddress;
            try
            {

                ipAddress = context.Request.Headers["HTTP_X_FORWARDED_FOR"]; ;

                if (string.IsNullOrEmpty(ipAddress) || (ipAddress.ToLower() == "unknown") || ipAddress.Length > 45)
                    ipAddress = context.Request.Headers["REMOTE_ADDR"];
            }
            catch (Exception ex)
            {
                ipAddress = "Invalid IP:" + ex.Message;
            }

            return ipAddress;
        }
    }
}
