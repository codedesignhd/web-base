using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeDesign.Models
{
    public class ToDo : BaseModel
    {
        public string title { get; set; }
        public TrangThaiThucHien trang_thai_thuc_hien { get; set; } = TrangThaiThucHien.OPEN;
        public long thoi_gian_ket_thuc { get; set; } = Utils.DateTimeUtils.TimeInEpoch();
    }
}
