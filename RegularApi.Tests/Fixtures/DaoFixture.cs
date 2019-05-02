using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using RegularApi.Domain.Model;
using RegularApi.Tests.Dao;

namespace RegularApi.Tests.Fixtures
{
    public class DaoFixture : BaseDaoIT
    {
        public async Task<Application> CreateApplication(string name)
        {
            var collection = GetCollection<Application>("applications");

            var application = new Application
            {
                Name = name
            };

            await collection.InsertOneAsync(application);

            return application;
        }

        public async Task<Application> GetApplicationById(ObjectId id)
        {
            var collection = GetCollection<Application>("applications");

            var filter = new FilterDefinitionBuilder<Application>().Where(application => application.Id.Equals(id));
            var cursor = await collection.FindAsync(filter);

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<DeploymentTemplate> GetDeploymentTemplateByIdAsync(ObjectId id)
        {
            var collection = GetCollection<DeploymentTemplate>("deploymentTemplates");

            var filter = new FilterDefinitionBuilder<DeploymentTemplate>()
                .Where(deploymentTemplate => deploymentTemplate.Id.Equals(id));

            var cursor = await collection.FindAsync(filter);

            return await cursor.FirstOrDefaultAsync();                
        }

        public async Task<DeploymentTemplate> CreateDeploymentTemplateAsync(string name)
        {
            var collection = GetCollection<DeploymentTemplate>("deploymentTemplates");
            var template = ModelFixture.BuildDeploymentTemplate(name);
           
            await collection.InsertOneAsync(template);

            return template;
        }
    }
}