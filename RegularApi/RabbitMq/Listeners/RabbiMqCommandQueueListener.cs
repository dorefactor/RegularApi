using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RegularApi.RabbitMq.Listeners
{
    public class RabbiMqCommandQueueListener : IRabbitMqMessageListener
    {
        private readonly ILogger _logger;
        
        public RabbiMqCommandQueueListener(ILogger<RabbiMqCommandQueueListener> logger) 
        {
            _logger = logger;
        }

        public void OnMessage(string message)
        {
            _logger.LogInformation("message received: {0}", message);
        }
    }
}