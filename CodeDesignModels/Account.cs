using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public class Account : ModelBase
    {
        public string username { get; set; }
        public string avatar { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public long dob { get; set; }
        public Role role { get; set; }
        public long last_login { get; set; }
        public RefreshToken refresh_token { get; set; }

        public bool IsValidRefreshToken()
        {
            return refresh_token != null
                && !string.IsNullOrWhiteSpace(refresh_token.token)
                && refresh_token.expires > Utilities.DateTimeUtils.TimeInEpoch();
        }
    }

    public class RefreshToken
    {
        public string token { get; set; }
        public long expires { get; set; }
        public long created_date { get; set; }
        public string ip { get; set; }
    }
}
