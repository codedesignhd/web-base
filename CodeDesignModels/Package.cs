using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public class Package : ModelBase
    {
        public string name { get; set; }
        public string description { get; set; }
        public int duration { get; set; }
        public DurationUnit duration_unit { get; set; }
        public decimal price { get; set; }
        public decimal price_discount { get; set; }
        public int order { get; set; }
    }
}
