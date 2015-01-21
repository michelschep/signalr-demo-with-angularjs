namespace DemoSignalR.MessageHandling
{
    public static class MessageExtensions
    {
        public static T PayloadAs<T>(this Message message)
        {
            return (T) message.Payload;
        }
    }
}