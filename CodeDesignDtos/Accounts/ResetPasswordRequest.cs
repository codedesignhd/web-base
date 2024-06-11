using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignDtos.Accounts
{
    public class ResetPasswordRequest
    {
        public string new_password { get; set; }
        public string re_new_password { get; set; }

        /// <summary>
        /// Là username đã được encode
        /// </summary>
        public string token { get; set; }
    }
}
