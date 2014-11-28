using System;
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

                while (true)
                {
                    var info = Console.ReadKey();
                    string c = (info.KeyChar == '\r') ? "<br>" : info.KeyChar.ToString();

                    if (info.KeyChar == '\r')
                        Console.WriteLine();

                    var message = new DemoMessage(c);

                    DemoHub.HubContext.Clients.All.sendDemoHubMessage(message);
                }
            }
        }
    }
}
