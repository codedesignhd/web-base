using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignKafka.Consumer
{
    public interface IMessageStream
    {
        void Publish(Message message);
        void Subscribe(string subscriberName, Action<Message> action);
    }
}
