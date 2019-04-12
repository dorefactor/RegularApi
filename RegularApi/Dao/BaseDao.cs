using LanguageExt;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace RegularApi.Dao
{
    public abstract class BaseDao
    {
        private readonly string _databaseName;
        private readonly string _collectionName;
        private readonly IMongoClient _mongoClient;
        
        protected BaseDao(IMongoClient mongoClient, string databaseName, string collectionName)
        {
            _databaseName = databaseName;
            _collectionName = collectionName;
            _mongoClient = mongoClient;
        }

        protected static Option<T> OfNullable<T>(T value)
        {
            return value.IsNull() ? Option<T>.None : Option<T>.Some(value);
        }

        protected IMongoCollection<T> GetCollection<T>()
        {
            return GetCollection<T>(_collectionName);
        }
        
        protected IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return GetDatabase().GetCollection<T>(collectionName);
        }
        
        private IMongoDatabase GetDatabase()
        {
            return _mongoClient.GetDatabase(_databaseName);
        }

        
    }
}