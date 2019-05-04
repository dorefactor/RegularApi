using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Dao
{
    public abstract class BaseDaoIT : BaseIT
    {
        private const string MongoDatabaseName = "regularOrchestrator";

        protected void DropCollection(string name)
        {
            GetDatabase().DropCollection(name);
        }

        protected IMongoCollection<T> GetCollection<T>(string name)
        {
            return GetDatabase().GetCollection<T>(name);
        }

        protected T GetDao<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }

        protected DaoFixture GetDaoFixture()
        {
            return GetDao<DaoFixture>();
        }

        private IMongoDatabase GetDatabase()
        {
            return MongoClient.GetDatabase(MongoDatabaseName);
        }
    }
}