using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegularApi.Configurations;
using RegularApi.Controllers;
using RegularApi.Controllers.Dashboard;

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

            // RabbitMQ services
            RabbitMqServiceConfig.AddConnectionFactory(services);
            RabbitMqServiceConfig.AddRabbitMqTemplate(services);
            RabbitMqServiceConfig.AddCommandQueueListener(services);

            // MongoDb services
            MongoServiceConfig.AddDaos(services);

            // Services
            ServiceConfig.AddApplicationServices(services);

            //services.AddMvc().AddApplicationPart(Assembly.Load(new AssemblyName("RegularApi.Controllers")));

            services.AddMvc().AddApplicationPart(typeof(AbstractController).Assembly);

            return services.BuildServiceProvider();
        }

        public override void Configure(IApplicationBuilder app)
        {
            base.Configure(app);
       }
    }
}