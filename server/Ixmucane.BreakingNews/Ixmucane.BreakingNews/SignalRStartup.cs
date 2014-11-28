using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;

namespace DemoSignalR
{
    public class SignalRStartup
    {
        public static IAppBuilder App = null;

        public void Configuration(IAppBuilder app)
        {
            // End point for client to connect to.
            // /signalr/hubs returns js meta data for hubs
            app.Map("/signalr", map =>
            {
                // Allow cross site calls
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
}