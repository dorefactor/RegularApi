using DataProtection;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace RegularApi.Configurations
{
    public static class DataProtectionConfig
    {
        public static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();
            
            var client = new MongoClient(configuration["RD_DPAPI_CONNECTION_STRING"]);
            var database = client.GetDatabase(configuration["RD_DPAPI_DATABASE"]);

            services.AddDataProtection()
                .SetApplicationName("RegularApi")
                .PersistKeysToMongoDb(database, configuration["RD_DPAPI_COLLECTION"]);

            var purpose = "DoRefactor.Deployment.Secrets";
            services.UseProtectorByAttribute(purpose);
            
            return services;
        }
    }
}