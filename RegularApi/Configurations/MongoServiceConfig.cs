using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RegularApi.Dao;

namespace RegularApi.Configurations
{
    public static class MongoServiceConfig
    {
        public static IServiceCollection AddMongoClient(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            var settings = new MongoClientSettings
            {
                UseSsl = false,
                Server = MongoServerAddress.Parse(configuration["MongoDb:Server"]),
                Credential = MongoCredential.CreateCredential(configuration["MongoDb:Database"],
                    configuration["MONGO_USER"], configuration["MONGO_PASSWORD"])
            };


            services.AddTransient<IMongoClient>(mongoClient => new MongoClient(settings));

            return services;
        }

        public static IServiceCollection AddDaos(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = provider.GetRequiredService<IConfiguration>();

            var mongoClient = provider.GetRequiredService<IMongoClient>();
            var databaseName = configuration["MongoDb:Database"];

            services.AddTransient<IApplicationDao>(applicationDao => new ApplicationDao(mongoClient, databaseName, "applications"));
            services.AddTransient<IDeploymentTemplateDao>(deploymentTemplateDao => new DeploymentTemplateDao(mongoClient, databaseName));

            return services;
        }
    }
}