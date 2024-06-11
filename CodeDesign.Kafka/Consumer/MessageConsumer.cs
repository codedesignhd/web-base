using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignKafka.Consumer
{
    public class MessageConsumer : IMessageConsumer
    {
        private ConsumerConfig config = null;
        public MessageConsumer()
        {

        }
        public void Listen(string topic)
        {
            throw new NotImplementedException();
        }
    }
}
