using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegularApi.TestServers.Tests;

namespace RegularApi.Tests
{
    public abstract class IntegrationTestBase
    {
        protected TestServer TestServer;
        protected HttpClient HttpClient;

        protected IServiceProvider ServiceProvider;

        protected void CreateTestServer()
        {
            TestServer = new TestServer(CreateHostBuilder());
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

        private static IWebHostBuilder CreateHostBuilder()
        {
            AddEnvironmentVariables();

            return new WebHostBuilder()
                .UseConfiguration(CreateConfiguration())
                .UseEnvironment("Development")
                .UseKestrel()
                .UseStartup<Startup>();
        }

        private static void AddEnvironmentVariables()
        {
            Environment.SetEnvironmentVariable("RABBIT_USER", "guest");
            Environment.SetEnvironmentVariable("RABBIT_PASSWORD", "guest");
            
            // Environment.SetEnvironmentVariable("MONGO_DATABASE", "regularOrchestrator");
            // Environment.SetEnvironmentVariable("MONGO_USER", "root");
            // Environment.SetEnvironmentVariable("MONGO_PASSWORD", "r00t");
        }

        private static IDictionary<string, string> AddInMemorySettings()
        {
            return new Dictionary<string, string>
            {
                { "RabbitMq:Server", "rabbitmq-host" },
                { "RabbitMq:Exchange", "regular-deployer-exchange" },
                { "RabbitMq:CommandQueue", "com.dorefactor.deploy.command" }
                // { "MongoDb:Server", "mongodb-host" },
                // { "MongoDb:Database", "regularOrchestrator" }
            };
        }
    }
}