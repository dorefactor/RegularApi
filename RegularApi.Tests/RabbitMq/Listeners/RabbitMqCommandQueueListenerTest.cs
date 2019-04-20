using FluentAssertions;
using NUnit.Framework;
using RegularApi.RabbitMq.Listener;

namespace RegularApi.Tests.RabbitMq.Listeners
{
    public class RabbitMqCommandQueueListenerTest : BaseIT
    {
        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public void TestCommandListener()
        {
            var commandListener = (RabbiMqCommandQueueListener) ServiceProvider.GetService(typeof(RabbitMqMessageListener));

            commandListener.Should().NotBeNull();
            commandListener.GetConsumerTag().Should().NotBeNullOrEmpty();
        }
    }
}