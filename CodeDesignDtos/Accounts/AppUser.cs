using CodeDesignModels;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;

namespace CodeDesignDtos.Accounts
{
    public class AppUser
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public Role Role { get; set; }
        public List<int> Props { get; set; }
        public bool IsAuthenticated { get; set; }
        //public static AppUser From(Account acc)
        //{
        //    if (acc != null)
        //    {
        //        return new AppUser()
        //        {
        //            Username = acc.username,
        //            Fullname = acc.fullname,
        //            Email = acc.email,
        //            Role = acc.role,
        //            Props = acc.thuoc_tinh,
        //        };
        //    }
        //    return default;
        //}
    }
}
