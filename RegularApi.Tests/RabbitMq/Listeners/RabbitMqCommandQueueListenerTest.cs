using NUnit.Framework;
using RegularApi.RabbitMq.Listener;

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
            var commandListener = (RabbiMqCommandQueueListener) ServiceProvider.GetService(typeof(RabbitMqMessageListener));

            Assert.NotNull(commandListener);
            Assert.IsNotEmpty(commandListener.GetConsumerTag());
        }
    }
}