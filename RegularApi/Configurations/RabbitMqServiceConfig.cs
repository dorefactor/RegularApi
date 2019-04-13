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
        public static void AddConnectionFactory(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = (IConfiguration) provider.GetService(typeof(IConfiguration));

            services.AddSingleton<IConnectionFactory>(option => new ConnectionFactory
            {
                HostName = configuration["RabbitMq:Server"],
                UserName = configuration["RABBIT_USER"],
                Password = configuration["RABBIT_PASSWORD"],
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            });
        }

        public static void AddRabbitMqTemplate(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = (IConfiguration) provider.GetService(typeof(IConfiguration));
            var connectionFactory = (IConnectionFactory) provider.GetService(typeof(IConnectionFactory));
            var loggerFactory = (ILoggerFactory) provider.GetService(typeof(ILoggerFactory));

            var exchange = configuration["RabbitMq:Exchange"];
            var queue = configuration["RabbitMq:CommandQueue"];

            services.AddSingleton<IRabbitMqTemplate>(new RabbitMqTemplate(loggerFactory, connectionFactory, exchange, queue));
        }

        public static void AddCommandQueueListener(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var connectionFactory = (IConnectionFactory) provider.GetService(typeof(IConnectionFactory));
            var configuration = (IConfiguration) provider.GetService(typeof(IConfiguration));
            var loggerFactory = (ILoggerFactory) provider.GetService(typeof(ILoggerFactory));
            var queue = configuration["RabbitMq:CommandQueue"];

            services.AddSingleton<RabbitMqMessageListener>(new RabbiMqCommandQueueListener(loggerFactory, connectionFactory, queue));
        }
    }
}