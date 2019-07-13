using Mongo2Go;
using MongoDB.Driver;

namespace DoRefactor.Tests.AspNetCore.DataProtection
{
    public abstract class BaseDatabase 
    {
        internal static readonly string DatabaseName = "test-key-database";
        internal static readonly string CollectionName = "key-collection";

        internal static MongoDbRunner MongoDbRunner;
        internal static IMongoClient MongoClient;

        internal static void CreateMongoDbServer()
        {
            MongoDbRunner = MongoDbRunner.Start();
            MongoClient = new MongoClient(MongoDbRunner.ConnectionString);
        }

        internal static void ReleaseMongoDbServer()
        {
            MongoDbRunner.Dispose();
        }
    }
}