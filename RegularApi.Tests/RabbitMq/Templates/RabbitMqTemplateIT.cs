using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RegularApi.RabbitMq.Templates;

namespace RegularApi.Tests.RabbitMq.Templates
{
    public class RabbitMqTemplateIT : BaseIT
    {
        private IRabbitMqTemplate _rabbitMqTemplate;

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();

            _rabbitMqTemplate = (IRabbitMqTemplate)ServiceProvider.GetRequiredService(typeof(IRabbitMqTemplate));
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public void TestSendMessage()
        {
            _rabbitMqTemplate.Should().NotBeNull();
            _rabbitMqTemplate.SendMessage("test-message");
        }
    }
}