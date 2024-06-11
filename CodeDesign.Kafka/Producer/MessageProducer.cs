using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignKafka.Producer
{
    public class MessageProducer
    {
        IProducer<Null, string> producer;
        ProducerConfig config = null;
        public MessageProducer()
        {
            config = new ProducerConfig
            {
                BootstrapServers = CodeDesignUtilities.ConfigurationManager.AppSettings["Kafka:Server"]
            };
            producer = new ProducerBuilder<Null, string>(config).Build();
        }
        void Produce()
        {

        }
    }
}
