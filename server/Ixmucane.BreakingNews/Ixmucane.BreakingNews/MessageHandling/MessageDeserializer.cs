using System;
using System.Collections.Generic;
using DemoSignalR.Messages;
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

        public Tuple<Type, object> Deserialize(string json)
        {
            var message = (dynamic)JsonConvert.DeserializeObject(json, new JsonSerializerSettings
            {
                
            });

            var messageId = Convert.ChangeType(message.messageId, typeof(Guid));
            _log.Info("MessageId [{0}]", messageId);

            var messageTypeAlias = (string) message.messageType;
            _log.Info("MessageTypeAlias [{0}]", messageTypeAlias);

            var messageType = MessageTypeFor(messageTypeAlias);
            _log.Info("MessageType [{0}]", messageType);

            return Tuple.Create(messageType, Activator.CreateInstance(messageType));
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