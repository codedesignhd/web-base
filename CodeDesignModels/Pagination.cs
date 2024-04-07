using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Models
{
    public class Pagination
    {
        public int page { get; set; }
        public long total { get; set; }
        public int page_size { get; set; }

        public int total_page
        {
            get
            {
                if (page_size > 0)
                {
                    return (int)Math.Ceiling((decimal)total / page_size);
                }
                return 0;
            }
        }
    }
}
