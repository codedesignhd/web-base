using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.ES.Models
{
    public class ScrollResult<T> where T : class
    {
        public List<T> Documents { get; set; }
        public long Total { get; set; }
        public string ScrollId { get; set; }
    }
}
