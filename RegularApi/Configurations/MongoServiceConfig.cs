using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RegularApi.Dao;

namespace RegularApi.Configurations
{
    public static class MongoServiceConfig
    {
        public static void AddMongoClient(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = (IConfiguration)provider.GetService(typeof(IConfiguration));

            var settings = new MongoClientSettings
            {
                UseSsl = false,
                Server = MongoServerAddress.Parse(configuration["MongoDb:Server"]),
                Credential = MongoCredential.CreateCredential(configuration["MongoDb:Database"],
                    configuration["MongoDb:User"], configuration["MongoDb:Password"])
            };


            services.AddTransient<IMongoClient>(mongoClient => new MongoClient(settings));
        }

        public static void AddDaos(IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var configuration = (IConfiguration)provider.GetService(typeof(IConfiguration));

            var mongoClient = (IMongoClient)provider.GetService(typeof(IMongoClient));
            var databaseName = configuration["MongoDb:Database"];

            services.AddTransient<IApplicationDao>(applicationDao => new ApplicationDao(mongoClient, databaseName, "applications"));
        }
    }
}