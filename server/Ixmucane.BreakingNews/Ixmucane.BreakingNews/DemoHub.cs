using System.Threading.Tasks;
using DemoSignalR.MessageHandling;
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
            var message = _deserializer.Deserialize(json);
            Handle(message);
        }

        void Handle(Message message)
        {
            var messageWithContext = new Envelope<Message>(message)
                .WithMeta("connectionId", Context.ConnectionId);

            MessageHandlingProcess.Instance.Handle(messageWithContext);
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
