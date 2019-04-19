using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RegularApi.RabbitMq.Listener
{
    public abstract class RabbitMqMessageListener : IMessageListener
    {
        protected string ConsumerTag;

        private readonly ILogger _logger;

        public abstract void OnMessage(string message);

        public string GetConsumerTag()
        {
            return ConsumerTag;
        }

        protected RabbitMqMessageListener(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger(GetType());
        }

        protected string AddQueueListener(IModel channel, string queue)
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = Encoding.UTF8.GetString(ea.Body);
                _logger.LogInformation("message received from queue: {0}", queue);

                // do something
                OnMessage(body);

                channel.BasicAck(ea.DeliveryTag, false);
            };
                            
            return channel.BasicConsume(queue, false, consumer);            
        }     
        
        protected static IModel CreateConnection(IConnectionFactory connectionFactory)
        {
            var connection = connectionFactory.CreateConnection();
            return connection.CreateModel();
        }
    }
}