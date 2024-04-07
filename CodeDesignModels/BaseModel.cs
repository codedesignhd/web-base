using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public class BaseModel
    {
        public string id { get; set; }
        public string nguoi_tao { get; set; } = Constants.DEFAULT_USER;
        public string nguoi_sua { get; set; }
        public long ngay_tao { get; set; } = Utils.DateTimeUtils.TimeInEpoch();
        public long ngay_sua { get; set; }
        public TrangThaiDuLieu trang_thai { get; set; } = TrangThaiDuLieu.ALL;
        public List<int> thuoc_tinh { get; set; } = new List<int>();
    }

}
