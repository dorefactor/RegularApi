using Mongo2Go;
using MongoDB.Driver;

namespace RegularApi.TestServers.Tests
{
    public class MongoServerTest<T>
    {
        internal static MongoDbRunner _mongoDbRunner;
        internal static string _mongoDatabaseName = "regularOrchestrator";
        internal static string _mongoCollectionName = "application";
        internal static IMongoCollection<T> _mongoCollection;
        internal static IMongoDatabase _mongoDatabase;
        internal static IMongoClient _mongoClient;

        internal void CreateConnection()
        {
            _mongoDbRunner = MongoDbRunner.Start(singleNodeReplSet: true);
            _mongoClient = new MongoClient(_mongoDbRunner.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(_mongoDatabaseName);
            _mongoCollection = _mongoDatabase.GetCollection<T>(_mongoCollectionName);
        }
    }
}