﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Requests
{
    public class PagingRequest : RequestBase
    {
        public int page { get; set; }
        public int page_size { get; set; }
    }
}
