using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace RegularApi.Tests.Dao
{
    public abstract class DaoTestBase : IntegrationTestBase
    {
        protected IMongoCollection<T> GetCollection<T>(string name)
        {
            var configuration = (IConfiguration) ServiceProvider.GetService(typeof(IConfiguration));
            var mongoClient = (IMongoClient) ServiceProvider.GetService(typeof(IMongoClient));

            var databaseName = configuration["MongoDb:Database"];
            var database = mongoClient.GetDatabase(databaseName);

            return database.GetCollection<T>(name);
        }
        
        protected T GetDao<T>()
        {
            return (T) ServiceProvider.GetService(typeof(T));
        }
    }
}