using System.Collections.Generic;
using System.Threading.Tasks;
using DataProtection.Protectors;
using LanguageExt;
using MongoDB.Driver;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public class ApplicationDao : BaseDao, IApplicationDao
    {
        public const string CollectionName = "applications";

        private readonly IMongoCollection<Application> _collection;

        public ApplicationDao(IMongoClient mongoClient, IProtector protector, string databaseName) 
            : base(mongoClient, protector, databaseName, CollectionName)
        {
            _collection = GetCollection<Application>();
        }

        public async Task<IList<Application>> GetAllAsync()
        {
            var cursor = await _collection.FindAsync(FilterDefinition<Application>.Empty);

            var data = _protector.UnprotectObject(await cursor.ToListAsync());
            
            return data;
        }

        public async Task<Option<Application>> GetByNameAsync(string name)
        {
            var filter = new FilterDefinitionBuilder<Application>().Where(app => name.Equals(app.Name));
            var cursor = await _collection.FindAsync(filter);

            var data = _protector.UnprotectObject(await cursor.FirstOrDefaultAsync());
            
            return OfNullable(data);
        }

        public async Task<Option<Application>> SaveAsync(Application application) // Change to Save
        {
            var protectedApp = _protector.ProtectObject(application);
            
            await _collection.InsertOneAsync(protectedApp);

            return OfNullable(application);
        }
    }
}