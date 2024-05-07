using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Accounts
{
    public class UpdateUserInfoRequest
    {
        public string username { get; set; }
        public string fullname { get; set; }
        public string dob { get; set; }
    }
}
