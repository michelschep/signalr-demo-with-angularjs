namespace DemoSignalR.Support
{
    public interface ILog
    {
        void Info(string format, params object[] args);
        void Info(object message);
    }
}