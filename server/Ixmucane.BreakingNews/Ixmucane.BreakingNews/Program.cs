using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;

namespace Ixmucane.BreakingNews
{
    class Program
    {
        static void Main(string[] args)
        {
            //var hubContext = GlobalHost.ConnectionManager.GetHubContext<BreakingNewsHub>();

            using (WebApp.Start<SignalRStartup>("http://*:8088/"))
            {
                while (true)
                {
                    var info = Console.ReadKey();
                    string c = (info.KeyChar == '\r') ? "<br>" : info.KeyChar.ToString();

                    if (info.KeyChar == '\r')
                        Console.WriteLine();

                    //string c = info.KeyChar.ToString();
                    var message = new BreakingNewsMessage(c);

                    BreakingNewsHub.HubContext.Clients.All.sendBreakingNews(message);
                }

                Console.WriteLine("Press [enter] to quit...");
                Console.ReadLine();
            }
        }
    }

    public class BreakingNewsMessage
    {
        public BreakingNewsMessage(string message)
        {
            Body = message;
            CreationDate = DateTime.Now.ToString();
        }

        public string CreationDate { get; set; }

        public string Body { get; set; }
    }
    public class SignalRStartup
    {
        public static IAppBuilder App = null;

        public void Configuration(IAppBuilder app)
        {
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration
                {
                    EnableDetailedErrors = true,
                    EnableJSONP = true
                };

                map.RunSignalR(hubConfiguration);
            });
        }
    }

    public class BreakingNewsHub : Hub
    {
        public BreakingNewsHub()
        {
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Connection!");
            return base.OnConnected();
        }

        public static IHubContext HubContext
        {
            get
            {
                if (_context == null)
                    _context = GlobalHost.ConnectionManager.GetHubContext<BreakingNewsHub>();

                return _context;
            }
        }
        static IHubContext _context = null;
    }
}
