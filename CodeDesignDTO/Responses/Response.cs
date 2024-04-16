using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Responses
{
    public sealed class Response : ResponseBase
    {
        public Response()
        {

        }
        public Response(bool success, string message) : base(success, message)
        {

        }
    }

    public sealed class Response<T> : ResponseBase
    {
        public T data { get; set; }
    }
}
