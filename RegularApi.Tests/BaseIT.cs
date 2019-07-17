using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using RegularApi.Configurations;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests
{
    public abstract class BaseIT
    {
        internal static MongoDbRunner DbRunner;
        internal static IMongoClient MongoClient;

        private TestServer _testServer;
        
        protected HttpClient HttpClient;
        protected IServiceProvider ServiceProvider;

        internal static void CreateMongoDbServer()
        {
            // 27018 initial port for mongo2go in unit test mode
            WaitForOpenPort(27018);
            DbRunner = MongoDbRunner.Start();
            MongoClient = new MongoClient(DbRunner.ConnectionString);
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
            DbRunner.Dispose();
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
                     
                     // DPAPI
                     services.AddCustomDataProtection();
                     
                     services.AddSingleton<DaoFixture, DaoFixture>();
                 })
                 .UseStartup<TestStartup>();
        }

        private static void AddEnvironmentVariables()
        {
            // DPAPI
            Environment.SetEnvironmentVariable("RD_DPAPI_CONNECTION_STRING", DbRunner.ConnectionString);
            Environment.SetEnvironmentVariable("RD_DPAPI_DATABASE", "keyStorage");
            Environment.SetEnvironmentVariable("RD_DPAPI_COLLECTION", "keys");
            
            Environment.SetEnvironmentVariable("RABBIT_USER", "guest");
            Environment.SetEnvironmentVariable("RABBIT_PASSWORD", "guest");
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

        private static void WaitForOpenPort(int port)
        {
            var available = false;
            while (!available) {

                using(TcpClient tcpClient = new TcpClient())
                {
                    try {
                        tcpClient.Connect("127.0.0.1", port);
                    } catch (Exception) {
                        available = true;
                    }
                }            

            }
        }        
    }
}