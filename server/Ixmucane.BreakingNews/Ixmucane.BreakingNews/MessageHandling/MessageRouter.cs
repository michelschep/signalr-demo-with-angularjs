using System;

namespace DemoSignalR.MessageHandling
{
    public class MessageRouter
    {
        public void Handle(Type messageType, object message)
        {
            throw new NotSupportedException(string.Format("No handling for type [{0}] message [{1}]", messageType, message));
        }
    }
}