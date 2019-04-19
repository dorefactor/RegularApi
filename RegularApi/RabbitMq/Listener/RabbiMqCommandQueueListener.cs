using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RegularApi.RabbitMq.Listener
{
    public class RabbiMqCommandQueueListener : RabbitMqMessageListener
    {
        public string LastReceivedMessage { get; set; }

        private readonly ILogger _logger;
        private readonly IModel _channel;

        public RabbiMqCommandQueueListener(ILoggerFactory loggerFactory, IConnectionFactory connectionFactory, string queue) : base(loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<RabbiMqCommandQueueListener>();
            _channel = CreateConnection(connectionFactory); 
            ConsumerTag = AddQueueListener(_channel, queue);
        }

        public override void OnMessage(string message)
        {
            LastReceivedMessage = message;
            _logger.LogInformation("message received: {0}", message);
        }
    }
}