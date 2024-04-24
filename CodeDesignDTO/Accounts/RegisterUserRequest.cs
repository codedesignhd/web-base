using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CodeDesign.Dtos.Accounts
{
    public class RegisterUserRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
    }
}
