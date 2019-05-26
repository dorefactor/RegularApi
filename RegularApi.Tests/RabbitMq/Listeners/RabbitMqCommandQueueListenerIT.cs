using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using Moq;
using NUnit.Framework;
using RabbitMQ.Client;
using RegularApi.RabbitMq.Listener;

namespace RegularApi.Tests.RabbitMq.Listeners
{
    public class RabbitMqCommandQueueListenerIT : BaseIT
    {
        private Mock<ILogger<RabbiMqCommandQueueListener>> _logger;
        private Mock<IConnectionFactory> _connectionFactory;
        private Mock<RegularApi.Factories.IHttpClientFactory> _httpClientFactory;
        private Mock<IConnection> _connection;
        private Mock<IModel> _model;
        private Mock<HttpClient> _httpClient;

        private IMessageListener _rabbiMqCommandQueueListener;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<RabbiMqCommandQueueListener>>();
            _connectionFactory = new Mock<IConnectionFactory>();
            _httpClientFactory = new Mock<RegularApi.Factories.IHttpClientFactory>();
            _connection = new Mock<IConnection>();
            _model = new Mock<IModel>();
            _httpClient = new Mock<HttpClient>();

            _connectionFactory.Setup(_ => _.CreateConnection()).Returns(_connection.Object);
            _connection.Setup(_ => _.CreateModel()).Returns(_model.Object);
            _httpClientFactory.Setup(_ => _.CreateWithBasicAuth()).Returns(_httpClient.Object);

            _rabbiMqCommandQueueListener = new RabbiMqCommandQueueListener(
                _logger.Object,
                _connectionFactory.Object,
                _httpClientFactory.Object,
                "queue");
        }

        [TearDown]
        public void TearDown()
        {
            _logger.VerifyNoOtherCalls();
            _connectionFactory.VerifyNoOtherCalls();
            _httpClientFactory.VerifyNoOtherCalls();
            _connection.VerifyNoOtherCalls();
        }

        [Test]
        public async Task TestOnMessageAsync()
        {
            var message = "asdf";

            await _rabbiMqCommandQueueListener.OnMessageAsync(message);

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _connectionFactory.Verify(_ => _.CreateConnection());
            _connection.Verify(_ => _.CreateModel());
            _httpClientFactory.Verify(_ => _.CreateWithBasicAuth());
        }
    }
}