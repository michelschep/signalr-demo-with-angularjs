using System;
using System.Threading;
using DemoSignalR.MessageHandling;
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

            var demoHubFactory = new Func<IHubContext>(() => GlobalHost.ConnectionManager.GetHubContext<DemoHub>());

            // booh! used by hub; rather find a way to have signalr inject this instance into hubs it creates
            var messageHandling = new MessageHandlingProcess(env => HandleMessage(env, demoHubFactory));
            messageHandling.Start();

            MessageHandlingProcess.Instance = messageHandling;

            using (WebApp.Start<SignalRStartup>("http://*:8088/"))
            {
                log.Info("SignalR push service started!");


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

        static void HandleMessage(Envelope<Message> envelope, Func<IHubContext> demoHubFactory)
        {
            var random = new Random();
            Thread.Sleep(random.Next(1000, 10*1000));
            
            Logs.As("Handling").Info(envelope);

            var connectionId = envelope.Meta["connectionId"];
            var response = new { messageId = envelope.Payload.MessageId};

            demoHubFactory().Clients.Client(connectionId).handle(response);
        }
    }
}
