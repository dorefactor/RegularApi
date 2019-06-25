using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests
{
    public abstract class BaseIT
    {
        internal static MongoDbRunner MongoDbRunner;
        internal static IMongoClient MongoClient;

        protected TestServer TestServer;
        protected HttpClient HttpClient;
        protected IServiceProvider ServiceProvider;

        internal static void CreateMongoDbServer()
        {
            MongoDbRunner = MongoDbRunner.Start();
            MongoClient = new MongoClient(MongoDbRunner.ConnectionString);
        }

        protected void CreateTestServer()
        {
            TestServer = new TestServer(CreateWebHostBuilder());

            ServiceProvider = TestServer.Host.Services;

            HttpClient = TestServer.CreateClient();
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected void ReleaseMongoDbServer()
        {
            MongoDbRunner.Dispose();
        }

        private static IConfiguration CreateConfigurationBuilder()
        {
            return new ConfigurationBuilder()
                .AddInMemoryCollection(SetInMemorySettings())
                .AddEnvironmentVariables()
                .Build();
        }

        private static IWebHostBuilder CreateWebHostBuilder()
        {
            AddEnvironmentVariables();

            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(CreateConfigurationBuilder())
                .UseEnvironment("Development")
                .ConfigureServices(services =>
                 {
                     services.AddSingleton(MongoClient);
                     services.AddSingleton<DaoFixture, DaoFixture>();
                 })
                 .UseStartup<TestStartup>();
        }

        private static void AddEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("RABBIT_USER", "guest");
            Environment.SetEnvironmentVariable("RABBIT_PASSWORD", "guest");
            
            Environment.SetEnvironmentVariable("MONGO_USER", "root");
            Environment.SetEnvironmentVariable("MONGO_PASSWORD", "r00t");
        }

        private static IDictionary<string, string> SetInMemorySettings()
        {
            return new Dictionary<string, string>
            {
                { "RabbitMq:Server", "rabbitmq-host" },
                { "RabbitMq:Exchange", "regular-deployer-exchange" },
                { "RabbitMq:CommandQueue", "com.dorefactor.deploy.command" },
                { "MongoDb:Database", "regularOrchestrator" }
            };
        }
    }
}