using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RegularApi.RabbitMq.Listeners;

namespace RegularApi.RabbitMq.Templates
{
    public class RabbitMqTemplate : IRabbitMqTemplate
    {
        public string ConsumerTag { get; }

        private readonly ILogger<RabbitMqTemplate> _logger;
        private readonly IModel _channel;

        private readonly IRabbitMqMessageListener _messageListener;

        public RabbitMqTemplate(IConnectionFactory connectionFactory, 
            IConfiguration configuration, 
            IRabbitMqMessageListener messageListener,
            ILogger<RabbitMqTemplate> logger)
        {
           _channel = CreateConnection(connectionFactory, configuration["RabbitMq:Exchange"], configuration["RabbitMq:CommandQueue"]);
           ConsumerTag = AddQueueListener(_channel, configuration["RabbitMq:CommandQueue"]);
           _messageListener = messageListener;
           _logger = logger;
        }

        public void SendMessage(string exchange, string queue, string message)
        {
            _logger.LogInformation("send message: {0} to exchange: {1} and route: {2}", message, exchange, queue);

            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange:exchange,
                routingKey: queue, body: body);
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
            channel.ExchangeDeclare(exchange: exchange, type: "direct", durable: true, autoDelete: false);
            channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false);
            channel.QueueBind(queue: queue, exchange: exchange, routingKey: queue);
        }

        private string AddQueueListener(IModel channel, string queue)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
                            {
                                var body = Encoding.UTF8.GetString(ea.Body);
                                _logger.LogInformation("message received from queue: {0}", queue);

                                // do something
                                _messageListener.OnMessage(body);

                                channel.BasicAck(ea.DeliveryTag, false);
                            };
                            
            return channel.BasicConsume(queue, false, consumer);            
        }     
    }
}