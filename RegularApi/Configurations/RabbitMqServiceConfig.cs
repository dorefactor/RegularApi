using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RegularApi.RabbitMq.Listener;
using RegularApi.RabbitMq.Templates;

namespace RegularApi.Configurations
{
    public static class RabbitMqServiceConfig
    {
        public static IServiceCollection AddConnectionFactory(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            services.AddTransient<IConnectionFactory>(option => new ConnectionFactory
            {
                HostName = configuration["RabbitMq:Server"],
                UserName = configuration["RABBIT_USER"],
                Password = configuration["RABBIT_PASSWORD"],
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(5)
            });

            return services;
        }

        public static IServiceCollection AddRabbitMqTemplate(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionFactory = provider.GetRequiredService<IConnectionFactory>();
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();

            var exchange = configuration["RabbitMq:Exchange"];
            var queue = configuration["RabbitMq:CommandQueue"];

            services.AddTransient<IRabbitMqTemplate>(rabbitMqTemplate => new RabbitMqTemplate(loggerFactory, connectionFactory, exchange, queue));

            return services;
        }

        public static IServiceCollection AddCommandQueueListener(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();
            var connectionFactory = provider.GetRequiredService<IConnectionFactory>();
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var queue = configuration["RabbitMq:CommandQueue"];

            services.AddTransient<RabbitMqMessageListener>(rabbiMqCommandQueueListener => new RabbiMqCommandQueueListener(loggerFactory, connectionFactory, queue));

            return services;
        }
    }
}