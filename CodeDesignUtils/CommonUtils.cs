using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CodeDesign.Utilities
{
    public abstract class CommonUtils
    {
        public static string GetIpAddress(HttpContext context)
        {
            string ipAddress;
            try
            {
                ipAddress = context.Connection.RemoteIpAddress.ToString();
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
