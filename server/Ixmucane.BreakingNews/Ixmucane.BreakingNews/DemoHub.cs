using System;
using System.Threading.Tasks;
using DemoSignalR.MessageHandling;
using Microsoft.AspNet.SignalR;

namespace DemoSignalR
{
    public class DemoHub : Hub
    {
        readonly Action<string> _handler;

        public DemoHub()
        {
            var deserializer = new MessageDeserializer();
            //new MessageRouter().Handle;
            _handler = message =>
            {
                Console.WriteLine("Received message [{0}]", message);
                var typedMessage = deserializer.Deserialize(message);
                Console.WriteLine("Received type [{0}] message [{1}]", typedMessage.Item1, typedMessage.Item2);
            };
        }

        public void Handle(string json)
        {
//            var clientMessage = new ClientMessage(Context.ConnectionId, jsonMessage);
//            Clients.Others.sendMessageFromClient(clientMessage);
            _handler(json);
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Connected [{0}]", Context.ConnectionId);
            
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            Console.WriteLine("Reconnected [{0}]", Context.ConnectionId);

            return base.OnReconnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine("Disconnected [{0}]", Context.ConnectionId);

            return base.OnDisconnected(stopCalled);
        }

        public static IHubContext HubContext
        {
            get
            {
                if (_context == null)
                    _context = GlobalHost.ConnectionManager.GetHubContext<DemoHub>();

                return _context;
            }
        }
        static IHubContext _context = null;
    }
}