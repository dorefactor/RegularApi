using DataProtection.Protectors;
using DataProtection.Repository;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DataProtection
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

        public static IServiceCollection UseProtectorByAttribute (this IServiceCollection services, string purpose)
        {
            var provider = services.BuildServiceProvider();

            var protectionProvider = provider.GetRequiredService<IDataProtectionProvider>();
            var dataProtector = protectionProvider.CreateProtector(purpose);
            
            services.AddSingleton<IProtector>(new Protector(dataProtector));
            
            return services;
        }
    }
}