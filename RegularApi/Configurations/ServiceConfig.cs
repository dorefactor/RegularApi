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
            services.AddTransient<ITransformer<ApplicationSetupView, ApplicationSetup>>(_ => new ApplicationSetupTransformer());
            services.AddTransient<ITransformer<ApplicationView, Application>>(_ =>
                                                                                    new ApplicationTransformer(_.GetRequiredService<ITransformer<ApplicationSetupView, ApplicationSetup>>()));
            services.AddTransient<ITransformer<DeploymentTemplateView, DeploymentTemplate>>(_ =>
                                                                                        new DeploymentTemplateTransformer(_.GetRequiredService<ITransformer<ApplicationSetupView, ApplicationSetup>>()));
            services.AddTransient<ITransformer<DeploymentOrderView, DeploymentOrder>>(_ =>
                                                                                        new DeploymentOrderTransformer(_.GetRequiredService<ITransformer<ApplicationSetupView, ApplicationSetup>>()));

            // Services
            services.AddTransient(deploymentService => new DeploymentService(loggerFactory, deploymentTemplateDao, deploymentOrderDao, rabbitTemplate));
            services.AddTransient(applicationService => new ApplicationService(applicationDao));
            services.AddTransient(deploymentTemplateService => new DeploymentTemplateService(loggerFactory, deploymentTemplateDao));

            return services;
        }
    }
}
