using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using MongoDB.Driver;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public class ApplicationDao : BaseDao, IApplicationDao
    {
        public static readonly string ApplicationCollectionName = "applications";

        private readonly IMongoCollection<Application> _collection;

        public ApplicationDao(IMongoClient mongoClient, string databaseName) 
            : base(mongoClient, databaseName, ApplicationCollectionName)
        {
            _collection = GetCollection<Application>();
        }

        public async Task<IList<Application>> GetAllAsync()
        {
            var cursor = await _collection.FindAsync(FilterDefinition<Application>.Empty);

            return await cursor.ToListAsync();
        }

        public async Task<Option<Application>> GetByNameAsync(string name)
        {
            var filter = new FilterDefinitionBuilder<Application>().Where(app => name.Equals(app.Name));
            var cursor = await _collection.FindAsync(filter);

            return OfNullable(await cursor.FirstOrDefaultAsync());
        }

        public async Task<Option<Application>> SaveAsync(Application application) // Change to Save
        {
            await _collection.InsertOneAsync(application);

            return OfNullable(application);
        }
    }
}