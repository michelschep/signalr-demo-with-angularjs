using System;
using System.Threading;
using DemoSignalR.Support;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Hosting;

namespace DemoSignalR
{
    class Program
    {
        static void Main()
        {
            var log = Logs.As<Program>();

            log.Info("Starting SignalR push service...");

            // booh! used by hub; rather find a way to have signalr inject this instance into hubs it creates
            var messageHandling = new MessageHandlingProcess(envelope => Logs.As("Handling").Info(envelope));
            messageHandling.Start();

            MessageHandlingProcess.Instance = messageHandling;

            using (WebApp.Start<SignalRStartup>("http://*:8088/"))
            {
                log.Info("SignalR push service started!");

                var demoHubFactory = new Func<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<DemoHub>());

                string line;
                while ((line = Console.ReadLine()) != null)
                {
                    if (string.Equals("exit", line, StringComparison.OrdinalIgnoreCase))
                    {
                        log.Info("exiting...");
                        break;
                    }

                    try
                    {
                        var messageId = Guid.Parse(line);
                        var message = new { messageId };

                        log.Info("sending [{0}]", messageId);

                        demoHubFactory().Clients.All.handle(message);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine(ex);
                    }
                }
            }

            messageHandling.Stop();
            Thread.Sleep(250);
        }
    }
}
