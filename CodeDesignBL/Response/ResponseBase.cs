﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.BL.Response
{
    public abstract class ResponseBase
    {
        public bool success { get; set; }
        public string message { get; set; }

        public ResponseBase()
        {

        }
        public ResponseBase(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }
    }

    public sealed class Response : ResponseBase
    {
        public Response()
        {

        }
        public Response(bool success, string message) : base(success, message)
        {

        }
        public Response(KeyValuePair<bool, string> result) : base(result.Key, result.Value)
        {

        }
    }

    public sealed class Response<T> : ResponseBase where T : class
    {
        public T data { get; set; }
    }

    public sealed class PagedResponse<T> : ResponseBase where T : class
    {
        public long total { get; set; }
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public List<T> data { get; set; } = new List<T>();
    }
}
