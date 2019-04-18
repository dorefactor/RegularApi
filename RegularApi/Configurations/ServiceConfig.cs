using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RegularApi.Dao;
using RegularApi.RabbitMq.Templates;
using RegularApi.Services;

namespace RegularApi.Configurations
{
    public static class ServiceConfig
    {
        public static void AddApplicationServices(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            // Dependencies
            var loggerFactory = (ILoggerFactory)provider.GetService(typeof(ILoggerFactory));
            var applicationDao = (IApplicationDao)provider.GetService(typeof(IApplicationDao));
            var rabbitTemplate = (IRabbitMqTemplate)provider.GetService(typeof(IRabbitMqTemplate));

            services.AddTransient(deploymentService => new DeploymentService(loggerFactory, applicationDao, rabbitTemplate));
            services.AddSingleton(applicationSetupService => new ApplicationSetupService(loggerFactory, applicationDao));
        }
    }
}
