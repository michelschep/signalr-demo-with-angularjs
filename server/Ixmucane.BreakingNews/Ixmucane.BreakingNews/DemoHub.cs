using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DemoSignalR.MessageHandling;
using DemoSignalR.Messages;
using DemoSignalR.Support;
using Microsoft.AspNet.SignalR;

namespace DemoSignalR
{
    public class DemoHub : Hub
    {
        readonly ILog _log = Logs.As<DemoHub>();
        readonly MessageDeserializer _deserializer = new MessageDeserializer();
        
        public void Handle(string json)
        {
            Handle(_deserializer.Deserialize(json));
        }

        void Handle(Tuple<Type, object> typedMessage)
        {
            var handlers = new Dictionary<Type, Action<object>>
            {
                {typeof(Query), HandleAs<Query>(Logs.As("Handling").Info)}
            };

            Action<object> handler;
            if (!handlers.TryGetValue(typedMessage.Item1, out handler))
                throw new NotSupportedException(string.Format("No handler for message of type [{0}]", typedMessage.Item1));

            handler(typedMessage.Item2);
        }

        Action<object> HandleAs<T>(Action<T> handler)
        {
            return message => handler((T) message);
        }

        public override Task OnConnected()
        {
            _log.Info("Connected [{0}]", Context.ConnectionId);
            
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            _log.Info("Reconnected [{0}]", Context.ConnectionId);

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            _log.Info("Disconnected [{0}]", Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }
    }
}