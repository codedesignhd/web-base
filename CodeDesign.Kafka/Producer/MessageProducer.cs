using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesign.Kafka.Producer
{
    public class MessageProducer
    {
        IProducer<Null, string> producer;
        ProducerConfig config = null;
        public MessageProducer()
        {
            config = new ProducerConfig
            {
                BootstrapServers = Utilities.ConfigurationManager.AppSettings["Kafka:Server"]
            };
            producer = new ProducerBuilder<Null, string>(config).Build();
        }
        void Produce()
        {

        }
    }
}
