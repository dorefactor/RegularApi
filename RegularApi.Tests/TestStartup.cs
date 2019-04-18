using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegularApi.Configurations;

namespace RegularApi.Tests
{
    public class TestStatup : Startup
    {
        //protected static MongoDbRunner MongoDbRunner;

        public TestStatup(IConfiguration configuration) : base(configuration)
        {
            //MongoDbRunner = MongoDbRunner.Start();
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // RabbitMQ services
            RabbitMqServiceConfig.AddConnectionFactory(services);
            RabbitMqServiceConfig.AddRabbitMqTemplate(services);
            RabbitMqServiceConfig.AddCommandQueueListener(services);

            // MongoDb services
            //services.AddTransient<IMongoClient>(mongoClient => new MongoClient(MongoDbRunner.ConnectionString));
            MongoServiceConfig.AddDaos(services);

            // Services
            ServiceConfig.AddApplicationServices(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            return services.BuildServiceProvider();
        }
    }
}