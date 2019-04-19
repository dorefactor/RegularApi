using MongoDB.Driver;

namespace RegularApi.Tests.Dao
{
    public abstract class BaseDaoIT : BaseIT
    {
        private static string _mongoDatabaseName = "regularOrchestrator";

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
            return (T)ServiceProvider.GetService(typeof(T));
        }

        private IMongoDatabase GetDatabase()
        {
            return MongoClient.GetDatabase(_mongoDatabaseName);
        }
    }
}