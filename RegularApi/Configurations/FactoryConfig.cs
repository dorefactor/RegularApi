using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegularApi.Factories;

namespace RegularApi.Configurations
{
    public static class FactoryConfig
    {
        public static IServiceCollection AddHttpClientFactory(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            services.AddSingleton<IHttpClientFactory>(
                new HttpClientFactory(configuration["RD_BUILDER_URL"], configuration["RD_BUILDER_TOKEN"])
            );

            return services;
        }
    }
}
