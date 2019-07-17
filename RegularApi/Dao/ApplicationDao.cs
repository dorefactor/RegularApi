using System.Collections.Generic;
using System.Linq;
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

            var applications = await cursor.ToListAsync();

            var unprotectedApps = applications.Select(UnprotectApplication)
                .ToList();
            
            return unprotectedApps;
        }

        public async Task<Option<Application>> GetByNameAsync(string name)
        {
            var filter = new FilterDefinitionBuilder<Application>()
                .Where(app => name.Equals(app.Name));

            var cursor = await _collection.FindAsync(filter);
            var application = await cursor.FirstOrDefaultAsync();

            var unprotectedApplication = UnprotectApplication(application);
            
            return OfNullable(unprotectedApplication);
        }

        public async Task<Option<Application>> SaveAsync(Application application)
        {
            var protectedApp = ProtectApplication(application);
            
            await _collection.InsertOneAsync(protectedApp);

            return OfNullable(application)
                .Map(app => {
                    app.Id = protectedApp.Id;
                    return app;
                });
        }

        private Application UnprotectApplication(Application application)
        {
            return application == null ? null : _protector.UnprotectObject(application);
        }

        private Application ProtectApplication(Application application)
        {
            return application == null ? null : _protector.ProtectObject(application);
        }
    }
}