using Mongo2Go;
using MongoDB.Driver;

namespace DoRefactor.Tests.AspNetCore.DataProtection
{
    public abstract class BaseIT 
    {
        internal static MongoDbRunner MongoDbRunner;
        internal static IMongoClient MongoClient;

        internal static void CreateMongoDbServer()
        {
            MongoDbRunner = MongoDbRunner.Start();
            MongoClient = new MongoClient(MongoDbRunner.ConnectionString);
        }

        protected void ReleaseMongoDbServer()
        {
            MongoDbRunner.Dispose();
        }
    }
}