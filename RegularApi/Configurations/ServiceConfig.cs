using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;
using RegularApi.RabbitMq.Templates;
using RegularApi.Services;
using RegularApi.Transformers;

namespace RegularApi.Configurations
{
    public static class ServiceConfig
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            // Dependencies
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var rabbitTemplate = provider.GetRequiredService<IRabbitMqTemplate>();

            var applicationDao = provider.GetRequiredService<IApplicationDao>();
            var deploymentTemplateDao = provider.GetRequiredService<IDeploymentTemplateDao>();
            var deploymentOrderDao = provider.GetRequiredService<IDeploymentOrderDao>();

            // Transformers
            services.AddTransient<IApplicationTransformer>(applicationTransformer => new ApplicationTransformer());
            services.AddTransient<IDeploymentTemplateTransformer>(deploymentTemplateTransformer => new DeploymentTemplateTransformer());
            services.AddTransient<ITransformer<DeploymentOrderView, DeploymentOrder>>(transformer => new DeploymentOrderTransformer());

            // Services
            services.AddTransient(deploymentService => new DeploymentService(loggerFactory, deploymentTemplateDao, deploymentOrderDao, rabbitTemplate));
            services.AddTransient(applicationSetupService => new ApplicationSetupService(loggerFactory, applicationDao));
            services.AddTransient(deploymentTemplateService => new DeploymentTemplateService(loggerFactory, deploymentTemplateDao));

            return services;
        }
    }
}
