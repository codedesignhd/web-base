using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Pagination
{
    public class Pager
    {
        public int Page { get; set; }
        public long Total { get; set; }
        public int PageSize { get; set; }

        public int TotalPages
        {
            get
            {
                if (PageSize > 0)
                {
                    return (int)Math.Ceiling((decimal)Total / PageSize);
                }
                return 1;
            }
        }
    }
}
