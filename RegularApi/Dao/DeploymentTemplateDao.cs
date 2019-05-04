using System.Threading.Tasks;
using LanguageExt;
using MongoDB.Bson;
using MongoDB.Driver;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public class DeploymentTemplateDao : BaseDao, IDeploymentTemplateDao
    {
        public static readonly string DeploymentTemplateCollectionName = "deployments.templates";

        private readonly IMongoCollection<DeploymentTemplate> _collection;
        
        public DeploymentTemplateDao(IMongoClient mongoClient, string databaseName) 
            : base(mongoClient, databaseName, DeploymentTemplateCollectionName)
        {
            _collection = GetCollection<DeploymentTemplate>();
        }

        public async Task<DeploymentTemplate> NewAsync(DeploymentTemplate template)
        {
            await _collection.InsertOneAsync(template);

            return template;
        }

        public async Task<Option<DeploymentTemplate>> GetByNameAsync(string templateName)
        {
            var filter = new FilterDefinitionBuilder<DeploymentTemplate>()
                .Where(template => template.Name.Equals(templateName));

            var deploymentTemplate = await _collection.FindAsync(filter);

            return OfNullable(deploymentTemplate.FirstOrDefault());
        }

        public async Task<Option<DeploymentTemplate>> GetByIdAsync(ObjectId id)
        {
            var filter = new FilterDefinitionBuilder<DeploymentTemplate>()
                .Where(deploymentTemplate => deploymentTemplate.Id.Equals(id));

            var deploymentTemplateHolder = await _collection.FindAsync(filter);

            return OfNullable(deploymentTemplateHolder.FirstOrDefault());
        }
    }
}