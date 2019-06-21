using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RegularApi.Domain.Views.Drone;

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

            _httpClient = httpClientFactory.CreateWithBearerToken();

            AddQueueListener(_channel, queue);
        }

        public override async Task OnMessageAsync(string message)
        {
            _logger.LogInformation("message received: {0}", message);

            var buildsRequest = await _httpClient.GetAsync("api/repos/dorefactor/rd-jenkins-deployer/builds");

            if (buildsRequest.IsSuccessStatusCode)
            {
                var content = await buildsRequest.Content.ReadAsStringAsync();
                var buildList = JsonConvert.DeserializeObject<List<BuildView>>(content);
                var count = buildList.Count;

                await _httpClient.PostAsync("api/repos/dorefactor/rd-jenkins-deployer/builds/" + count + "?DEPLOYMENT_ORDER_ID=" + message, null);

                return;
            }

            _logger.LogError("can't get last deployment number for order: {0}", message);

            // await _httpClient.PostAsync("regular-deployer/job/deployer/buildWithParameters?DEPLOYMENT_ORDER_ID=" + message, null);
        }
    }
}