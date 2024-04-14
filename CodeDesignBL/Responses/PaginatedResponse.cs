using CodeDesign.Dtos.Requests;
using CodeDesign.ES.Models;
using System;
using System.Collections.Generic;

namespace CodeDesign.BL.Responses
{
    public class PaginatedResponse<TViewModel> : ResponseBase where TViewModel : class
    {
        public long total { get; set; }
        public List<TViewModel> data { get; set; }
        public int page { get; set; }
        public int page_size { get; set; }
        public int total_pages
        {
            get
            {
                if (page_size <= 0)
                    page_size = 1;
                return (int)Math.Ceiling((decimal)total / page_size);
            }
        }
        public string scroll_id { get; set; }

        public static PaginatedResponse<TViewModel> Create(PagingRequest request)
        {
            PaginatedResponse<TViewModel> response = new PaginatedResponse<TViewModel>();
            if (request != null)
            {
                response.page = request.page;
                response.page_size = request.page_size;
            }
            return response;
        }
    }
}
