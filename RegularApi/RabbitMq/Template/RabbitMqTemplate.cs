using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RegularApi.RabbitMq.Templates
{
    public class RabbitMqTemplate : IRabbitMqTemplate
    {
        private readonly ILogger<RabbitMqTemplate> _logger;
        private readonly IModel _channel;
        private readonly string _exchange;
        private readonly string _queue;

        public RabbitMqTemplate(ILoggerFactory loggerFactory, IConnectionFactory connectionFactory, string exchange, string queue)
        {
            _exchange = exchange;
            _queue = queue;
            _logger = loggerFactory.CreateLogger<RabbitMqTemplate>();
            _channel = CreateConnection(connectionFactory, exchange, queue);
        }

        public void SendMessage(string message)
        {
            _logger.LogInformation("send message: {0} to exchange: {1} and route: {2}", message, _exchange, _queue);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(_exchange,_queue, body: body);
        }

        private IModel CreateConnection(IConnectionFactory connectionFactory, string exchange, string queue)
        {
            var connection = connectionFactory.CreateConnection();
            var channel = connection.CreateModel();
            ResourceDeclare(channel, exchange, queue);

            return channel;
        }

        private void ResourceDeclare(IModel channel, string exchange, string queue)
        {
            channel.ExchangeDeclare(exchange, "direct", true);
            channel.QueueDeclare(queue, true,  false,  false);
            channel.QueueBind(queue, exchange, queue);
        }
    }
}