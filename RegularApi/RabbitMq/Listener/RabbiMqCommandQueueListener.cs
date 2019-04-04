using System;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RegularApi.RabbitMq.Listener
{
    public class RabbiMqCommandQueueListener : RabbitMqMessageListener
    {
        private readonly ILogger _logger;
        private readonly IModel _channel;
        public string LastReceivedMessage { get; set; }
        
        
        public RabbiMqCommandQueueListener(IConnectionFactory connectionFactory, string queue, ILogger<RabbiMqCommandQueueListener> logger) : base(logger)
        {
            _logger = logger;
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