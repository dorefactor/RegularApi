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

        private TestServer _testServer;
        
        protected HttpClient HttpClient;
        protected IServiceProvider ServiceProvider;

        internal static void CreateMongoDbServer()
        {
            MongoDbRunner = MongoDbRunner.Start();
            MongoClient = new MongoClient(MongoDbRunner.ConnectionString);
        }

        protected void CreateTestServer()
        {
            _testServer = new TestServer(CreateWebHostBuilder());

            ServiceProvider = _testServer.Host.Services;

            HttpClient = _testServer.CreateClient();
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected static void ReleaseMongoDbServer()
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
            // DPAPI
            Environment.SetEnvironmentVariable("RD_DPAPI_CONNECTION_STRING", MongoDbRunner.ConnectionString);
            Environment.SetEnvironmentVariable("RD_DPAPI_DATABASE", "keyStorage");
            Environment.SetEnvironmentVariable("RD_DPAPI_COLLECTION", "keys");
            
            Environment.SetEnvironmentVariable("RABBIT_USER", "xoom");
            Environment.SetEnvironmentVariable("RABBIT_PASSWORD", "xoom123");
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