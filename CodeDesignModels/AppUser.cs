using System.Collections.Generic;
using System.IO;
using System.Security.Principal;

namespace CodeDesign.Models
{
    public class AppUser
    {
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public List<int> Props { get; set; }
        public bool IsAuthenticated { get; set; }
        public static AppUser From(TaiKhoan tai_khoan)
        {
            if (tai_khoan != null)
            {
                return new AppUser()
                {
                    Username = tai_khoan.username,
                    Fullname = tai_khoan.fullname,
                    Email = tai_khoan.email,
                    Role = tai_khoan.role,
                    Props = tai_khoan.thuoc_tinh,
                };
            }
            return default;
        }
    }
}
