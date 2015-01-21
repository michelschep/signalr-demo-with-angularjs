using System;
using System.Collections.Generic;
using DemoSignalR.Messages;
using DemoSignalR.Support;
using Newtonsoft.Json;

namespace DemoSignalR.MessageHandling
{
    /// <summary>
    /// Main entry point for handling incoming Json messages.
    /// </summary>
    public class MessageDeserializer
    {
        readonly ILog _log = Logs.As<MessageDeserializer>();

        readonly IDictionary<string, Type> _typeAliases;

        public MessageDeserializer()
        {
            _typeAliases = new Dictionary<string, Type>
            {
                {"query", typeof(Query)}
            };
        }

        public Message Deserialize(string json)
        {
            var message = (dynamic)JsonConvert.DeserializeObject(json);

            var messageId = Convert.ChangeType(message.messageId, typeof(Guid));
            var messageTypeAlias = (string) message.messageType;
            var messageType = MessageTypeFor(messageTypeAlias);
//            var payload = JsonConvert.DeserializeObject(message.Payload, messageType);

            _log.Info("MessageId [{0}]", messageId);
            _log.Info("MessageTypeAlias [{0}]", messageTypeAlias);
            _log.Info("MessageType [{0}]", messageType);

            return new Message(messageId, Activator.CreateInstance(messageType), messageType);
        }

        Type MessageTypeFor(string messageTypeAlias)
        {
            Type type;
            if (!_typeAliases.TryGetValue(messageTypeAlias, out type))
                throw new ArgumentOutOfRangeException("messageTypeAlias", string.Format("Message type alias [{0}] cannot be resolved to a known type.", messageTypeAlias));
            return type;
        }
    }
}