using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RegularApi.Configurations;
using RegularApi.RabbitMq.Listener;
using RegularApi.RabbitMq.Templates;

namespace RegularApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly ILoggerFactory _loggerFactory;

        public Startup(IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            // RabbitMQ services
            // RabbitMqServiceConfig.AddConnectionFactory(services, Configuration);
            // RabbitMqServiceConfig.AddRabbitMqTemplate(services, Configuration, _loggerFactory.CreateLogger<RabbitMqTemplate>());
            // RabbitMqServiceConfig.AddCommandQueueListener(services, Configuration, _loggerFactory.CreateLogger<RabbiMqCommandQueueListener>());

            // MongoDb services
            MongoServiceConfig.AddMongoClient(services, Configuration);
            MongoServiceConfig.AddDaos(services, Configuration);

            // Services
            ServiceConfig.AddApplicationServices(services);

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(builder =>
                        {
                            builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                        });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
