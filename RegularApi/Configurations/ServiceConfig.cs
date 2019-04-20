using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RegularApi.Dao;
using RegularApi.RabbitMq.Templates;
using RegularApi.Services;

namespace RegularApi.Configurations
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            // Dependencies
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var applicationDao = provider.GetRequiredService<IApplicationDao>();
            var rabbitTemplate = provider.GetRequiredService<IRabbitMqTemplate>();

            services.AddSingleton(deploymentService => new DeploymentService(loggerFactory, applicationDao, rabbitTemplate));
            services.AddSingleton(applicationSetupService => new ApplicationSetupService(loggerFactory, applicationDao));

            return services;
        }
    }
}
