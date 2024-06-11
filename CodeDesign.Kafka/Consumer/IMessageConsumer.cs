using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignKafka.Consumer
{
    public interface IMessageConsumer
    {
        void Listen(string topic);
    }
}
