using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Responses
{
    public class Response
    {
        public bool success { get; set; }
        public string message { get; set; }
        public Response()
        {

        }
        public Response(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }
    }

    public sealed class Response<T> : Response
    {
        public T data { get; set; }
        public Response()
        {

        }
        public Response(bool success, string message) : base(success, message)
        {

        }

    }
}
