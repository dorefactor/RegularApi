using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        private IWebHostBuilder CreateHostBuilder()
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
            Environment.SetEnvironmentVariable("RABBIT_HOST", "rabbitmq-host");
            Environment.SetEnvironmentVariable("RABBIT_USER", "xoom");
            Environment.SetEnvironmentVariable("RABBIT_PASSWORD", "xoom123");
        }

        private static IDictionary<string, string> AddInMemorySettings()
        {
            return new Dictionary<string, string>
            {
                { "RabbitMq:Exchange", "regular-deployer-exchange" },
                { "RabbitMq:CommandQueue", "com.dorefactor.deploy.command" }
            };
        }
    }
}