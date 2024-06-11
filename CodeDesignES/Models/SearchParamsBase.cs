using CodeDesignModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignES.Models
{
    public class SearchParamsBase
    {
        public string scroll_id { get; set; }
        public int page { get; set; } = 1;
        public int page_size { get; set; } = 10;
        public Dictionary<string, SortDir> sort { get; set; }
        public string[] fields { get; set; }

        public static SearchParamsBase Create(string scrollId, int page, int pageSize, string sortField, SortDir sortDir)
        {
            SearchParamsBase p = new SearchParamsBase()
            {
                scroll_id = scrollId,
                page = page,
                page_size = pageSize,
            };
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                p.sort = new Dictionary<string, SortDir>
                {
                    {sortField, sortDir }
                };
            }
            return p;
        }
    }
}
