using DemoSignalR.MessageHandling;
using DemoSignalR.Messages;
using NUnit.Framework;

namespace DemoSignalR.Test
{
    public class MessageDeserializerTest
    {
        [Test]
        public void can_deserialize_json_message_into_typed_equivalent()
        {
            var deserializer = new MessageDeserializer();
            var message = deserializer.Deserialize(@"
                {
                    ""messageId"": ""b9f39581-ebc5-4544-8a15-acd08b144fd7"",
                    ""messageType"": ""query""
                }"
            );
            Assert.That(message.Item2, Is.TypeOf<Query>(), "message");
        }
    }
}
