using System;
using System.Collections.Generic;
using System.Text;

namespace CodeDesignKafka.Consumer
{
    public class MessageStream : IMessageStream
    {
        public void Publish(Message message)
        {
            throw new NotImplementedException();
        }

        public void Subscribe(string subscriberName, Action<Message> action)
        {
            throw new NotImplementedException();
        }
    }
}
