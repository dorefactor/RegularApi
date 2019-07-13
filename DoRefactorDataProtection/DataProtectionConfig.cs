using DoRefactor.AspNetCore.DataProtection.Protector;
using DoRefactor.AspNetCore.DataProtection.Repository;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DoRefactor.AspNetCore.DataProtection
{
    public static class DataProtectionConfig
    {
        public static IDataProtectionBuilder PersistKeysToMongoDb(this  IDataProtectionBuilder builder, IMongoDatabase db, string collectionName)
        {
            builder.Services.Configure<KeyManagementOptions>(options => {
                options.XmlRepository = new MongoXmlRepository(db, collectionName);
            });
            
            return builder;
        }

        public static IServiceCollection UseProtectorByAttribute (this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();

            var dataProtector = provider.GetRequiredService<IDataProtector>();

            services.AddSingleton<IProtector>(new Protector.Protector(dataProtector));
            
            return services;
        }
    }
}