using System.Threading.Tasks;
using NUnit.Framework;
using RegularApi.RabbitMq.Templates;

namespace RegularApi.Tests.RabbitMq.Templates
{
    public class RabbitMqTemplateTest : IntegrationTestBase
    {

        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public void TestSendMessage()
        {
            var rabbitTemplate = (IRabbitMqTemplate) ServiceProvider.GetService(typeof(IRabbitMqTemplate));

            Assert.NotNull(rabbitTemplate);
            rabbitTemplate.SendMessage("test-message");
        }
    }
}