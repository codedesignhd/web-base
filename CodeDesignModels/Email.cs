using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace CodeDesign.Models
{
    public class Email : BaseModel
    {
        public string title { get; set; }
        public string body { get; set; }
        public byte so_lan_loi { get; set; }
        public string nguoi_nhan { get; set; }
        public TrangThaiMail trang_thai_gui { get; set; }
    }
}
