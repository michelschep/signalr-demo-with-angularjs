using System;

namespace DemoSignalR.MessageHandling
{
    public class Message
    {
        public readonly Guid MessageId;
        public readonly object Payload;
        public readonly Type PayloadType;

        public Message(Guid messageId, object payload, Type payloadType)
        {
            MessageId = messageId;
            Payload = payload;
            PayloadType = payloadType;
        }
    }
}