using System;

namespace DemoSignalR
{
    public interface ILog
    {
        void Info(string format, params object[] args);
        void Info(object message);
    }

    public class ConsoleLog : ILog
    {
        readonly string _name;

        public ConsoleLog(string name)
        {
            _name = name;
        }

        public void Info(string format, params object[] args)
        {
            Log(format, args);
        }

        public void Info(object message)
        {
            Log(message.ToString());
        }

        void Log(string format, params object[] args)
        {
            var msg = (args == null || args.Length == 0) ? format : string.Format(format, args);
            Console.WriteLine("{0} [{1}] {2}", DateTime.Now, _name, msg);
        }
    }

    public static class Logs
    {
        static readonly Func<string, ILog> LogFactory = name => new ConsoleLog(name);

        public static ILog As<T>()
        {
            return As(typeof (T).Name);
        }

        static ILog As(string name)
        {
            return LogFactory(name);
        }
    }
}