using System;

namespace DemoSignalR
{
    public class DemoMessage
    {
        public DemoMessage(string message)
        {
            Body = message;
            CreationDate = DateTime.Now.ToString();
        }

        public string CreationDate { get; set; }

        public string Body { get; set; }
    }
}