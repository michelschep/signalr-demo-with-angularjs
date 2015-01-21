using System;

namespace DemoSignalR.Support
{
    public static class Logs
    {
        static readonly Func<string, ILog> LogFactory = name => new ConsoleLog(name);

        public static ILog As<T>()
        {
            return As(typeof (T).Name);
        }

        public static ILog As(string name)
        {
            return LogFactory(name);
        }
    }
}