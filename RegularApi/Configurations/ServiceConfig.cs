using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;
using RegularApi.Protector;
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
            var rabbitTemplate = provider.GetRequiredService<IRabbitMqTemplate>();

            // Daos
            var applicationDao = provider.GetRequiredService<IApplicationDao>();
            var deploymentTemplateDao = provider.GetRequiredService<IDeploymentTemplateDao>();
            var deploymentOrderDao = provider.GetRequiredService<IDeploymentOrderDao>();
            
            // Protection Service
            var protectionProvider = provider.GetRequiredService<IDataProtectionProvider>();
            var realm = "DoRefactor.RegularDeployer.Secrets";

            services.AddSingleton<IProtector>(new Protector.Protector(protectionProvider, realm)); 
            
            // Transformers
            services.AddSingleton<ITransformer<ApplicationSetupView, ApplicationSetup>>(new ApplicationSetupTransformer());
            services.AddSingleton<ITransformer<ApplicationView, Application>>(_ =>
                                                                    new ApplicationTransformer(
                                                                        _.GetRequiredService<ITransformer<ApplicationSetupView, ApplicationSetup>>()));
            services.AddSingleton<ITransformer<DeploymentTemplateView, DeploymentTemplate>>(_ =>
                                                                        new DeploymentTemplateTransformer(
                                                                            _.GetRequiredService<ITransformer<ApplicationView, Application>>()));
            services.AddSingleton<ITransformer<DeploymentOrderView, DeploymentOrder>>(_ =>
                                                                        new DeploymentOrderTransformer(
                                                                            _.GetRequiredService<ITransformer<ApplicationView, Application>>()));
            // Services
            services.AddSingleton(new ApplicationService(applicationDao));
            services.AddSingleton(new DeploymentTemplateService(provider.GetRequiredService<ILogger<DeploymentTemplateService>>(), deploymentTemplateDao));
            services.AddSingleton(new DeploymentService(provider.GetRequiredService<ILogger<DeploymentService>>(), deploymentTemplateDao, deploymentOrderDao, rabbitTemplate));

            return services;
        }
    }
}
