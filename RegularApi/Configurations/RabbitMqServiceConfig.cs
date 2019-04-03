using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Logging;
using RegularApi.RabbitMq.Listener;
using RegularApi.RabbitMq.Templates;

namespace RegularApi.Configurations
{
    public static class RabbitMqServiceConfig
    {
        public static void AddConnectionFactory(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionFactory>(option => new ConnectionFactory
            {
                HostName = configuration["RABBIT_HOST"],
                UserName = configuration["RABBIT_USER"],
                Password = configuration["RABBIT_PASSWORD"],
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            });
        }

        public static void AddRabbitMqTemplate(IServiceCollection services, IConfiguration configuration, ILogger<RabbitMqTemplate> logger)
        {
            var provider = services.BuildServiceProvider();
            var connectionFactory = (IConnectionFactory) provider.GetService(typeof(IConnectionFactory));

            var exchange = configuration["RabbitMq:Exchange"];
            var queue = configuration["RabbitMq:CommandQueue"];

            services.AddSingleton<IRabbitMqTemplate>(new RabbitMqTemplate(connectionFactory, exchange, queue, logger));
        }

        public static void AddCommandQueueListener(IServiceCollection services, IConfiguration configuration,
            ILogger<RabbiMqCommandQueueListener> logger)
        {
            var provider = services.BuildServiceProvider();
            var connectionFactory = (IConnectionFactory) provider.GetService(typeof(IConnectionFactory));
            var queue = configuration["RabbitMq:CommandQueue"];

            services.AddSingleton<RabbitMqMessageListener>(new RabbiMqCommandQueueListener(connectionFactory, queue, logger));
        }
    }
}