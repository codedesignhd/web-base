using CodeDesignUtilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignModels
{
    public class ModelBase
    {
        public string id { get; set; }
        public string nguoi_tao { get; set; }
        public string nguoi_sua { get; set; }
        public long ngay_tao { get; set; } = DateTimeUtils.TimeInEpoch();
        public long ngay_sua { get; set; }
        public TrangThaiDuLieu trang_thai { get; set; } = TrangThaiDuLieu.All;
        public List<int> thuoc_tinh { get; set; } = new List<int>();
    }

}
