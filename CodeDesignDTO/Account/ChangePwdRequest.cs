using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Account
{
    public class ChangePwdRequest
    {
        public string OldPasword { get; set; }
        public string NewPassword { get; set; }
        public string ReNewPassword { get; set; }
    }
}
