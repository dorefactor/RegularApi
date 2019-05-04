using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
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

            RegisterConventions();

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

            services.AddTransient<IApplicationDao>(applicationDao => new ApplicationDao(mongoClient, databaseName));
            services.AddTransient<IDeploymentTemplateDao>(deploymentTemplateDao => new DeploymentTemplateDao(mongoClient, databaseName));
            services.AddTransient<IDeploymentOrderDao>(deploymentOrderDao => new DeploymentOrderDao(mongoClient, databaseName));

            return services;
        }

        private static void RegisterConventions()
        {
            // IgnoreIfNullConvention
            ConventionRegistry.Register("IgnoreIfNullConvention", new ConventionPack
            {
                new IgnoreIfNullConvention(true)
            }, filter => true);

            // EnumStringConvention
            ConventionRegistry.Register("EnumStringConvention", new ConventionPack
            {
                new EnumRepresentationConvention(BsonType.String)
            }, filter => true);
        }
    }
}