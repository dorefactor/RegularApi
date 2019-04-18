using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;

namespace RegularApi.Tests
{
    public abstract class BaseIT
    {
        protected static MongoDbRunner MongoDbRunner;

        protected TestServer TestServer;
        protected HttpClient HttpClient;
        protected IServiceProvider ServiceProvider;

        protected void CreateTestServer()
        {
            MongoDbRunner = MongoDbRunner.Start();

            TestServer = new TestServer(CreateWebHostBuilder());

            ServiceProvider = TestServer.Host.Services;
            HttpClient = TestServer.CreateClient();
        }

        private static IConfiguration CreateConfiguration()
        {
            AddEnvironmentVariables();

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
                              //services.AddTransient<IMongoClient>(mongoClient => new MongoClient(mongoUrlBuilder.ToMongoUrl()));

                          })
                .UseStartup<Startup>()
                          //.ConfigureTestServices(services =>
                          //{
                          //    services.AddTransient<IMongoClient>(mongoClient => new MongoClient(MongoDbRunner.ConnectionString));
                          //    //services.AddTransient<IMongoClient>(mongoClient => new MongoClient(mongoUrlBuilder.ToMongoUrl()));

                          //})
                          ;
        }

        private static void AddEnvironmentVariables()
        {
            //Environment.SetEnvironmentVariable("RABBIT_USER", "guest");
            //Environment.SetEnvironmentVariable("RABBIT_PASSWORD", "guest");
            
             Environment.SetEnvironmentVariable("MONGO_USER", "root");
             Environment.SetEnvironmentVariable("MONGO_PASSWORD", "r00t");
        }

        private static IDictionary<string, string> AddInMemorySettings()
        {
            return new Dictionary<string, string>
            {
                //{ "RabbitMq:Server", "rabbitmq-host" },
                //{ "RabbitMq:Exchange", "regular-deployer-exchange" },
                //{ "RabbitMq:CommandQueue", "com.dorefactor.deploy.command" }
                 { "MongoDb:Server", "mongodb-host" },
                 { "MongoDb:Database", "regularOrchestrator" }
            };
        }


    }
}