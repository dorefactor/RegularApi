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

        public async Task<IList<Application>> GetApplicationsAsync()
        {
            var cursor = await _collection.FindAsync(FilterDefinition<Application>.Empty);

            return await cursor.ToListAsync();
        }

        public async Task<Option<Application>> GetApplicationByNameAsync(string name)
        {
            var filter = new FilterDefinitionBuilder<Application>().Where(app => name.Equals(app.Name));
            var cursor = await _collection.FindAsync(filter);

            return OfNullable(await cursor.FirstOrDefaultAsync());
        }

        public async Task<Option<Application>> SaveApplicationSetup(Application application) // Change to Save
        {
            await _collection.InsertOneAsync(application);

            return OfNullable(application);
        }
    }
}