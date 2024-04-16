using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Dtos.Responses
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
}
