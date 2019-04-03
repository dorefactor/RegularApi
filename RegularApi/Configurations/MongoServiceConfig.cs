using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace RegularApi.Configurations
{
    public static class MongoServiceConfig
    {
        public static void AddMongoClient(IServiceCollection services, IConfiguration configuration)
        {
            var settings = new MongoClientSettings
            {
                UseSsl = false,
                Server = MongoServerAddress.Parse(configuration["MongoDb:Server"]),
                Credential = MongoCredential.CreateCredential(configuration["MongoDb:Database"], 
                    configuration["MONGO_USER"], configuration["MONGO_PASSWORD"])
            };

            services.AddSingleton<IMongoClient>(new MongoClient(settings));
        }
    }
}