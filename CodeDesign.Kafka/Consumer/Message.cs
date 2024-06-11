using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignKafka.Consumer
{
    public class Message
    {
        public string Topic { get; set; }
        public string Content { get; set; }
        public string ClientId { get; set; }
    }
}
