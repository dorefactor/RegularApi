using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace RegularApi.RabbitMq.Listener
{
    public class RabbiMqCommandQueueListener : RabbitMqMessageListener
    {
        private readonly ILogger _logger;
        private readonly IModel _channel;
        private readonly HttpClient _httpClient;

        public RabbiMqCommandQueueListener(ILogger<RabbiMqCommandQueueListener> logger,
                                           IConnectionFactory connectionFactory,
                                           Factories.IHttpClientFactory httpClientFactory,
                                           string queue) : base(logger)
        {
            _logger = logger;
            _channel = CreateConnection(connectionFactory);
            _httpClient = httpClientFactory.CreateWithBasicAuth();

            AddQueueListener(_channel, queue);
        }

        public override async Task OnMessageAsync(string message)
        {
            _logger.LogInformation("message received: {0}", message);

            await _httpClient.PostAsync("regular-deployer/job/deployer/buildWithParameters?DEPLOYMENT_ORDER_ID=" + message, null);
        }
    }
}