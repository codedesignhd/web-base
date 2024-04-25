using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Accounts
{
    public class ChangePwdRequest
    {
        public string old_password { get; set; }
        public string new_password { get; set; }
        public string re_new_password { get; set; }
    }
}
