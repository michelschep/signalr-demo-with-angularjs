using System;
using System.Threading;

namespace DemoSignalR.Support
{
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
            Console.WriteLine("({0}) {1} [{2}] {3}", Thread.CurrentThread.ManagedThreadId, DateTime.Now, _name, msg);
        }
    }
}