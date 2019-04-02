using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RegularApi.RabbitMq.Listeners
{
    public abstract class RabbitMqMessageListener
    {
        private readonly ILogger _logger;
        
        public string ConsumerTag { get; set; }

        protected RabbitMqMessageListener(ILogger<RabbitMqMessageListener> logger)
        {
            _logger = logger;
        }

        protected abstract void OnMessage(string message);
        
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
        
        protected IModel CreateConnection(IConnectionFactory connectionFactory)
        {
            var connection = connectionFactory.CreateConnection();
            return connection.CreateModel();
        }
        
    }
}