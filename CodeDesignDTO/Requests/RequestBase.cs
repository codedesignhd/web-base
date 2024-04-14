using CodeDesign.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Requests
{
    public class RequestBase
    {
        public string sort_field { get; set; }
        public SortDir sort_dir { get; set; }
        public string scroll_id { get; set; }
    }
}
