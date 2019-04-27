using System.Threading.Tasks;
using LanguageExt;
using MongoDB.Driver;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public class DeploymentTemplateDao : BaseDao, IDeploymentTemplateDao
    {
        private readonly IMongoCollection<DeploymentTemplate> _collection;
        
        public DeploymentTemplateDao(IMongoClient mongoClient, string databaseName) 
            : base(mongoClient, databaseName, "deploymentTemplates")
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
    }
}