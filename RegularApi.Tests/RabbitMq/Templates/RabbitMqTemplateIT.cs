using FluentAssertions;
using NUnit.Framework;
using RegularApi.RabbitMq.Templates;

namespace RegularApi.Tests.RabbitMq.Templates
{
    public class RabbitMqTemplateIT : BaseIT
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

            rabbitTemplate.Should().NotBeNull();
            rabbitTemplate.SendMessage("test-message");
        }
    }
}