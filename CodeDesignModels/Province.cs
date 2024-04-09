using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    /// <summary>
    /// Tỉnh thành, quận huyện
    /// </summary>
    public class Province
    {
        public string id { get; set; } = "";
        public string name { get; set; } = "";
        public string parent_id { get; set; } = "";
        public ProvinceType type { get; set; }
    }
}
