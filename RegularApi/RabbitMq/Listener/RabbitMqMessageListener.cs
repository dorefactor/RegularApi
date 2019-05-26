using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RegularApi.RabbitMq.Listener
{
    public abstract class RabbitMqMessageListener : IMessageListener
    {
        protected string ConsumerTag;
        private readonly ILogger _logger;

        public abstract Task OnMessageAsync(string message);

        public string GetConsumerTag()
        {
            return ConsumerTag;
        }

        protected RabbitMqMessageListener(ILogger logger)
        {
            _logger = logger;
        }

        protected string AddQueueListener(IModel channel, string queue)
        {
            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.Received += async (sender, @event) =>
            {
                var message = Encoding.UTF8.GetString(@event.Body);
                _logger.LogInformation("message received from queue: {0}", queue);

                // do something
                await OnMessageAsync(message);

                channel.BasicAck(@event.DeliveryTag, false);
            };

            return channel.BasicConsume(queue, false, consumer);
        }

        protected IModel CreateConnection(IConnectionFactory connectionFactory)
        {
            return connectionFactory
                        .CreateConnection()
                        .CreateModel();
        }
    }
}