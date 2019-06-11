using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegularApi.Configurations;
using RegularApi.Controllers;

namespace RegularApi.Tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration) : base(configuration)
        {
        }

        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            // Factories
            services.AddHttpClientFactory();

            // RabbitMQ services
            services.AddConnectionFactory();
            services.AddRabbitMqTemplate();
            services.AddCommandQueueListener();

            // MongoDb services
            services.AddDaos();

            // Services
            services.AddApplicationServices();

            services.AddMvc().AddApplicationPart(typeof(AbstractController).Assembly);

            return services.BuildServiceProvider();
        }
    }
}