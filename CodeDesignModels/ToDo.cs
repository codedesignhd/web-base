using CodeDesign.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeDesign.Models
{
    public class ToDo : ModelBase
    {
        public string title { get; set; }
        public TrangThaiThucHien trang_thai_thuc_hien { get; set; } = TrangThaiThucHien.OPEN;
        public long thoi_gian_ket_thuc { get; set; } = DateTimeUtils.TimeInEpoch();
    }
}
