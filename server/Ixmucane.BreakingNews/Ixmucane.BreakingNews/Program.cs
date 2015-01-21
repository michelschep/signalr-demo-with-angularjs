using System;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin.Hosting;

namespace DemoSignalR
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Starting SignalR push service...");

            using (WebApp.Start<SignalRStartup>("http://*:8088/"))
            {
                Console.WriteLine("SignalR push service started!");

                var demoHubFactory = new Func<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<DemoHub>());

                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    try
                    {
                        var messageId = Guid.Parse(line);
                        var message = new { messageId };

                        Console.WriteLine("sending [{0}]", messageId);

                        demoHubFactory().Clients.All.handle(message);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                }
            }
        }
    }
}
