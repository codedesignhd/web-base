using CodeDesign.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.ES.Models
{
    public class SearchResult<T> where T : ModelBase
    {
        public string scroll_id { get; set; }
        public long total { get; set; }
        public List<T> documents { get; set; } = new List<T>();
    }
}
