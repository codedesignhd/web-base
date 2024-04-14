using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Accounts
{
    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public bool is_remember { get; set; } = true;
        public string redirect_uri { get; set; } = "/";
    }
}
