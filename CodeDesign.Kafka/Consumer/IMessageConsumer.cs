using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Kafka.Consumer
{
    public interface IMessageConsumer
    {
        void Listen(string topic);
    }
}
