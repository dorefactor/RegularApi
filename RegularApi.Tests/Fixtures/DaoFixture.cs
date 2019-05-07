using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.Tests.Dao;

namespace RegularApi.Tests.Fixtures
{
    public class DaoFixture : BaseDaoIT
    {
        public async Task<Application> CreateApplicationAsync(string name)
        {
            var application = ModelFixture.BuildApplication(name);
            var collection = GetCollection<Application>(ApplicationDao.ApplicationCollectionName);

            await collection.InsertOneAsync(application);

            return application;
        }

        public async Task<Application> GetApplicationByIdAsync(ObjectId id)
        {
            var collection = GetCollection<Application>(ApplicationDao.ApplicationCollectionName);

            var filter = new FilterDefinitionBuilder<Application>().Where(application => application.Id.Equals(id));
            var cursor = await collection.FindAsync(filter);

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<DeploymentTemplate> GetDeploymentTemplateByIdAsync(ObjectId id)
        {
            var collection = GetCollection<DeploymentTemplate>(DeploymentTemplateDao.DeploymentTemplateCollectionName);

            var filter = new FilterDefinitionBuilder<DeploymentTemplate>()
                .Where(deploymentTemplate => deploymentTemplate.Id.Equals(id));

            var cursor = await collection.FindAsync(filter);

            return await cursor.FirstOrDefaultAsync();
        }

        public async Task<DeploymentTemplate> CreateDeploymentTemplateAsync(string name)
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate(name);
            var collection = GetCollection<DeploymentTemplate>(DeploymentTemplateDao.DeploymentTemplateCollectionName);

            await collection.InsertOneAsync(deploymentTemplate);

            return deploymentTemplate;
        }

        public async Task<DeploymentOrder> CreateDeploymentOrderAsync(string requestId)
        {
            await CreateApplicationAsync("application");
            await CreateDeploymentTemplateAsync("template");

            var deploymentOrder = ModelFixture.BuildDeploymentOrder(requestId);
            var collection = GetCollection<DeploymentOrder>(DeploymentOrderDao.DeploymentOrderCollectionName);

            await collection.InsertOneAsync(deploymentOrder);

            return deploymentOrder;
        }


        public async Task<DeploymentOrder> GetDeploymentOrderByIdAsync(string id)
        {
            var collection = GetCollection<DeploymentOrder>(DeploymentOrderDao.DeploymentOrderCollectionName);

            var filter = new FilterDefinitionBuilder<DeploymentOrder>()
                .Where(deploymentOrder => deploymentOrder.Id.Equals(new ObjectId(id)));

            var cursor = await collection.FindAsync(filter);

            return await cursor.FirstOrDefaultAsync();
        }
    }
}