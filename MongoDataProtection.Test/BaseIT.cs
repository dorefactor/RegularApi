using System;
using DoRefactor.Tests.AspNetCore.DataProtection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace DoRefactor.AspNetCore.DataProtection
{
    public class BaseIT
    {
        protected TestServer TestServer;

        protected IServiceProvider ServiceProvider;

        protected void CreateTestServer()
        {
            TestServer = new TestServer(CreateWebHostBuilder());
            ServiceProvider = TestServer.Host.Services;
        }

        private static IConfiguration CreateConfigurationBuilder()
        {
            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
        }

        private static IWebHostBuilder CreateWebHostBuilder()
        {
            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(CreateConfigurationBuilder())
                .UseEnvironment("Development")
                 .UseStartup<TestStartup>();
        }
        
    }
}