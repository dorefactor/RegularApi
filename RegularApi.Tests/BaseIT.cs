using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Mongo2Go;
using MongoDB.Driver;

namespace RegularApi.Tests
{
    public abstract class BaseIT
    {
        protected static MongoDbRunner MongoDbRunner;
        protected static IMongoClient MongoClient;

        protected TestServer TestServer;
        protected HttpClient HttpClient;
        protected IServiceProvider ServiceProvider;

        protected void CreateTestServer()
        {
            MongoDbRunner = MongoDbRunner.Start();
            MongoClient = new MongoClient(MongoDbRunner.ConnectionString);
            TestServer = new TestServer(CreateWebHostBuilder());
            ServiceProvider = TestServer.Host.Services;
            HttpClient = TestServer.CreateClient();
            //HttpClient.BaseAddress = new Uri("http://192.168.99.1:5000");
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            // To avoid hardcoding path to project, see: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing#integration-testing
            var integrationTestsPath = PlatformServices.Default.Application.ApplicationBasePath; // integration_tests/bin/Debug/netcoreapp2.0
            var applicationPath = Path.GetFullPath(Path.Combine(integrationTestsPath, "/Volumes/jealvarez/projects/RegularApi/RegularApi"));

            return WebHost.CreateDefaultBuilder()
                .UseConfiguration(CreateConfigurationBuilder())
                .UseEnvironment("Development")
                //.UseContentRoot(applicationPath)
                //.UseSolutionRelativeContentRoot("/Volumes/jealvarez/projects/RegularApi/")
                .ConfigureServices(services =>
                 {
                     services.AddTransient(mongoClient => MongoClient);
                 })
                 .UseStartup<TestStartup>();

            //return new WebHostBuilder()

                //.UseConfiguration(CreateConfigurationBuilder())
                //.UseEnvironment("Development")
                //.UseKestrel()
                ////.UseSolutionRelativeContentRoot(".")
                ////.UseContentRoot(Directory.GetCurrentDirectory())
                //.UseContentRoot(applicationPath)
                ////.UseUrls("http://*:5000")
                //.ConfigureServices(services =>
                //{
                //    services.AddTransient(mongoClient => MongoClient);
                //})
                 //.UseStartup<TestStartup>();

        }

        private static IDictionary<string, string> SetInMemorySettings()
        {
            return new Dictionary<string, string>
            {
                { "RabbitMq:Server", "rabbitmq-host" },
                { "RabbitMq:Exchange", "regular-deployer-exchange" },
                { "RabbitMq:CommandQueue", "com.dorefactor.deploy.command" },
                { "RabbitMq:User", "guest" },
                { "RabbitMq:Password", "guest" },
                { "MongoDb:Server", "mongodb-host" },
                { "MongoDb:Database", "regularOrchestrator" },
                { "MongoDb:User", "root" },
                { "MongoDb:Password", "r00t" }
            };
        }



        //private static string GetProjectPath(string solutionRelativePath, Assembly startupAssembly)
        //{
        //    var projectName = "RegistrationApplication";
        //    var applicationBasePath = AppContext.BaseDirectory;
        //    var directoryInfo = new DirectoryInfo(applicationBasePath);
        //    do
        //    {
        //        var solutionFileInfo = new FileInfo(Path.Combine(directoryInfo.FullName, SolutionName));
        //        if (solutionFileInfo.Exists)
        //        {
        //            return Path.GetFullPath(Path.Combine(directoryInfo.FullName, solutionRelativePath, projectName));
        //        }
        //        directoryInfo = directoryInfo.Parent;
        //    }
        //    while (directoryInfo.Parent != null);
        //    throw new NotImplementedException($"Solution root could not be located using application root {applicationBasePath}.");
        //}


    }
}