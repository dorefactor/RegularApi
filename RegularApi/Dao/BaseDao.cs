using DataProtection.Protectors;
using LanguageExt;
using MongoDB.Driver;

namespace RegularApi.Dao
{
    public abstract class BaseDao
    {
        private readonly string _databaseName;
        private readonly string _collectionName;
        private readonly IMongoClient _mongoClient;

        protected readonly IProtector _protector;

        protected BaseDao(IMongoClient mongoClient, IProtector protector, string databaseName, string collectionName)
        {
            _databaseName = databaseName;
            _collectionName = collectionName;
            _mongoClient = mongoClient;
            _protector = protector;
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
            var database = _mongoClient.GetDatabase(_databaseName);
            var collection = database.GetCollection<T>(collectionName);

            return collection;
        }
    }
}