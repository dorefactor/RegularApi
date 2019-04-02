using System.Threading;
using NUnit.Framework;
using RegularApi.RabbitMq.Listeners;
using RegularApi.RabbitMq.Templates;

namespace RegularApi.Tests.RabbitMq.Listeners
{
    public class RabbitMqCommandQueueListenerTest : IntegrationTestBase
    {
        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public void TestCommandListener()
        {
            var rabbitTemplate = (IRabbitMqTemplate) ServiceProvider.GetService(typeof(IRabbitMqTemplate));

            var commandListener = (RabbiMqCommandQueueListener) ServiceProvider.GetService(typeof(RabbitMqMessageListener));

            Assert.NotNull(commandListener);
            Assert.IsNotEmpty(commandListener.GetConsumerTag());

            const string message = "something in the wire";
            rabbitTemplate.SendMessage(message);
            
            Thread.Sleep(500);
            
            Assert.AreEqual(message, commandListener.LastReceivedMessage);
        }
    }
}