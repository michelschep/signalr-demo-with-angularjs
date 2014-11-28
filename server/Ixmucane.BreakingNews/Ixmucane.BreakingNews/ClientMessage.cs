namespace DemoSignalR
{
    public class ClientMessage
    {
        public ClientMessage(string name, string message)
        {
            Name = name;
            Message = message;
        }

        public string Name { get; set; }

        public string Message { get; set; }
    }
}