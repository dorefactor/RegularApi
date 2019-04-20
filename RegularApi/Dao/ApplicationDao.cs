using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using MongoDB.Driver;
using RegularApi.Dao.Model;

namespace RegularApi.Dao
{
    public class ApplicationDao : BaseDao, IApplicationDao
    {
        public ApplicationDao(IMongoClient mongoClient, string databaseName, string collectionName) : base(mongoClient, databaseName, collectionName)
        {
        }

        public async Task<IList<Application>> GetApplicationsAsync()
        {
            var collection = GetCollection<Application>();
            var cursor = await collection.FindAsync(FilterDefinition<Application>.Empty);

            return await cursor.ToListAsync();
        }

        public async Task<Option<Application>> GetApplicationByNameAsync(string name)
        {
            var collection = GetCollection<Application>();
            var filter = new FilterDefinitionBuilder<Application>().Where(app => name.Equals(app.Name));
            var cursor = await collection.FindAsync(filter);

            return OfNullable(await cursor.FirstOrDefaultAsync());
        }

        public async Task<Option<Application>> SaveApplicationSetup(Application application)
        {
            await GetCollection<Application>().InsertOneAsync(application);

            return OfNullable(application);
        }
    }
}