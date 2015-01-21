using System;
using Microsoft.AspNet.SignalR;
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

                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    try
                    {
                        var messageId = Guid.Parse(line);
                        var message = new { messageId };

                        Console.WriteLine("sending [{0}]", messageId);

                        GlobalHost.ConnectionManager.GetHubContext<DemoHub>().Clients.All.handle(message);
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
