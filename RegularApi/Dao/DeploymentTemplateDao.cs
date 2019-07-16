using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataProtection.Protectors;
using LanguageExt;
using MongoDB.Bson;
using MongoDB.Driver;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public class DeploymentTemplateDao : BaseDao, IDeploymentTemplateDao
    {
        public static readonly string CollectionName = "deploymentsTemplates";

        private readonly IMongoCollection<DeploymentTemplate> _collection;
        
        public DeploymentTemplateDao(IMongoClient mongoClient, IProtector protector, string databaseName) 
            : base(mongoClient, protector, databaseName, CollectionName)
        {
            _collection = GetCollection<DeploymentTemplate>();
        }

        public async Task<DeploymentTemplate> SaveAsync(DeploymentTemplate template)
        {
            var protectedTemplate = _protector.ProtectObject(template);
            await _collection.InsertOneAsync(protectedTemplate);

            template.Id = protectedTemplate.Id;
            return template;
        }

        public async Task<Option<DeploymentTemplate>> GetByNameAsync(string templateName)
        {
            var filter = new FilterDefinitionBuilder<DeploymentTemplate>()
                .Where(template => template.Name.Equals(templateName));

            var deploymentTemplate = await _collection.FindAsync(filter);

            return OfNullable(deploymentTemplate.FirstOrDefault())
                .Map(_protector.UnprotectObject);
        }

        public async Task<Option<DeploymentTemplate>> GetByIdAsync(ObjectId id)
        {
            var filter = new FilterDefinitionBuilder<DeploymentTemplate>()
                .Where(deploymentTemplate => deploymentTemplate.Id.Equals(id));

            var deploymentTemplateHolder = await _collection.FindAsync(filter);

            return OfNullable(deploymentTemplateHolder.FirstOrDefault())
                .Map(_protector.UnprotectObject);
        }

        public async Task<IList<DeploymentTemplate>> GetAllAsync()
        {
            var cursor = await _collection.FindAsync(FilterDefinition<DeploymentTemplate>.Empty);

            var templates = await cursor.ToListAsync();

            return templates.Select(_protector.UnprotectObject)
                .ToList();
        }
    }
}