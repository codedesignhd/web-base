using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public class TaiKhoan : BaseModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public Role role { get; set; }
        public long last_login { get; set; }
    }
}
