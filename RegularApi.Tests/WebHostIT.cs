using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using RegularApi.Configurations;

namespace RegularApi.Tests
{
    public abstract class WebHostI
    {
        protected static MongoDbRunner MongoDbRunner;

        protected TestServer TestServer;
        protected HttpClient HttpClient;
        protected IServiceProvider ServiceProvider { get; set; }

        protected void CreateTestServer()
        {

            MongoDbRunner = MongoDbRunner.Start();

            TestServer = new TestServer(CreateWebHostBuilder());

            ServiceProvider = TestServer.Host.Services;
            HttpClient = TestServer.CreateClient();
        }

        private static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(AddInMemorySettings())
                .AddEnvironmentVariables()
                .Build();
        }

        private static IWebHostBuilder CreateWebHostBuilder()
        {
            return new WebHostBuilder()
            .UseConfiguration(CreateConfiguration())
            .UseEnvironment("Development")
            .UseKestrel()
            .ConfigureServices(services =>
            //.ConfigureTestServices(services =>
            {
                services.AddTransient<IMongoClient>(mongoClient => new MongoClient(MongoDbRunner.ConnectionString));
            //    //services.AddTransient<IMongoClient>(mongoClient => new MongoClient(mongoUrlBuilder.ToMongoUrl()));

            })
            .UseStartup<TestStatup>()
            //.ConfigureTestServices(services =>
            //{
            //    services.AddTransient<IMongoClient>(mongoClient => new MongoClient(MongoDbRunner.ConnectionString));
            //    //services.AddTransient<IMongoClient>(mongoClient => new MongoClient(mongoUrlBuilder.ToMongoUrl()));

            //})
            ;


            //// Startup implements IStartup
            //var startup = new Startup(CreateConfiguration());

            //// copy existing config into memory
            //var existingConfig = new MemoryConfigurationSource();
            //existingConfig.InitialData = startup.Configuration.AsEnumerable();

            //// create new configuration from existing config
            //// and override whatever needed
            //var testConfigBuilder = new ConfigurationBuilder()
            //    .Add(existingConfig)
            //    ;


            //startup.Configuration = testConfigBuilder.Build();

            //var mongoUrlBuilder = new MongoUrlBuilder(MongoDbRunner.ConnectionString);
            //mongoUrlBuilder.DatabaseName = "regularOrchestrator";

            //var builder = new WebHostBuilder()
            //    .ConfigureTestServices(services =>
            //    {
            //        //services.AddTransient<IMongoClient>(mongoClient => new MongoClient(MongoDbRunner.ConnectionString));
            //        services.AddTransient<IMongoClient>(mongoClient => new MongoClient(mongoUrlBuilder.ToMongoUrl()));
            //        services.AddSingleton<IStartup>(startup);
            //    });

            ////var server = new TestServer(builder);
            //return builder;



        }

        private static IDictionary<string, string> AddInMemorySettings()
        {
            return new Dictionary<string, string>
            {
                { "RabbitMq:Server", "rabbitmq-host" },
                { "RabbitMq:Exchange", "regular-deployer-exchange" },
                { "RabbitMq:CommandQueue", "com.dorefactor.deploy.command" },
                { "RabbitMq:User", "guest" },
                { "RabbitMq:Password", "guest" },
                { "MongoDb:Server", "mongodb-host" },
                { "MongoDb:Database", "regularOrchestrator" }
            };
        }


    }
}