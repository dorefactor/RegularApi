using DoRefactor.AspNetCore.DataProtection.Repository;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DoRefactor.AspNetCore.DataProtection
{
    public static class MongoDataProtectionConfig
    {
        public static IDataProtectionBuilder PersistKeysToMongoDb(this  IDataProtectionBuilder builder, IMongoDatabase db, string collectionName)
        {
            builder.Services.Configure<KeyManagementOptions>(options => {
                options.XmlRepository = new MongoXmlRepository(db, collectionName);
            });
            
            return builder;
        }
    }
}