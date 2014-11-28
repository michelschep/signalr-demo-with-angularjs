using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace DemoSignalR
{
    public class DemoHub : Hub
    {
        public void SendMessage(string message)
        {
            var clientMessage = new ClientMessage(Context.ConnectionId, message);

            Clients.Others.sendMessageFromClient(clientMessage);
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