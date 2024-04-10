using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.ES.Models
{
    public class SearchResult<T>
    {
        public long total { get; set; }
        public List<T> documents { get; set; } = new List<T>();
    }
}
