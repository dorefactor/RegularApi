using System;
using DataProtection.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using static DataProtection.DataProtectionConfig;

namespace DoRefactor.Tests.AspNetCore.DataProtection
{
    public class TestStartup : BaseDatabase, IStartup
    {
        public void Configure(IApplicationBuilder app)
        {
            
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            CreateMongoDbServer();
            var database = MongoClient.GetDatabase(DatabaseName);
            services.AddSingleton(MongoClient);

            services.AddSingleton<IXmlRepository>(new MongoXmlRepository(database, CollectionName));

            services.AddDataProtection()
                .SetApplicationName("test-application")
                .PersistKeysToMongoDb(database, CollectionName);

            services.UseProtectorByAttribute("testPurpose");

            return services.BuildServiceProvider();
        }
    }
}