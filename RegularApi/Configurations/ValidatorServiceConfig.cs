using Microsoft.Extensions.DependencyInjection;
using RegularApi.Controllers.Deployment.Validators;

namespace RegularApi.Configurations
{
    public static class ValidatorServiceConfig
    {
        public static void AddControllerValidators(IServiceCollection services)
        {
            services.AddSingleton<DeploymentRequestValidator, DeploymentRequestValidator>();
        }
    }
}