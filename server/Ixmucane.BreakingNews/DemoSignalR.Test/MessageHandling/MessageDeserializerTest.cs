using System;
using DemoSignalR.MessageHandling;
using DemoSignalR.Messages;
using NUnit.Framework;

namespace DemoSignalR.Test.MessageHandling
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
            Assert.That(message.MessageId, Is.EqualTo(Guid.Parse("b9f39581-ebc5-4544-8a15-acd08b144fd7")), "message id");
            Assert.That(message.PayloadType, Is.EqualTo(typeof(Query)), "message payload type");
            Assert.That(message.Payload, Is.TypeOf<Query>(), "message payload");
        }
    }
}
