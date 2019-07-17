using System.Threading.Tasks;
using DataProtection.Protectors;
using MongoDB.Bson;
using MongoDB.Driver;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.Enums;
using RegularApi.Tests.Dao;

namespace RegularApi.Tests.Fixtures
{
    public class DaoFixture : BaseDaoIT
    {
        private readonly IProtector _protector;

        public DaoFixture(IProtector protector)
        {
            _protector = protector;
        }
        
        public async Task<Application> CreateApplicationAsync(string name, ApplicationType applicationType)
        {
            var application = ModelFixture.BuildApplication(name, applicationType);
            var collection = GetCollection<Application>(ApplicationDao.CollectionName);

            var protectedApplication = _protector.ProtectObject(application);

            await collection.InsertOneAsync(protectedApplication);

            return application;
        }

        public async Task<Application> GetApplicationByIdAsync(ObjectId id)
        {
            var collection = GetCollection<Application>(ApplicationDao.CollectionName);

            var filter = new FilterDefinitionBuilder<Application>().Where(application => application.Id.Equals(id));
            var cursor = await collection.FindAsync(filter);

            var app = await cursor.FirstOrDefaultAsync();

            return _protector.UnprotectObject(app);
        }

        public async Task<DeploymentTemplate> GetDeploymentTemplateByIdAsync(ObjectId id)
        {
            var collection = GetCollection<DeploymentTemplate>(DeploymentTemplateDao.CollectionName);

            var filter = new FilterDefinitionBuilder<DeploymentTemplate>()
                .Where(deploymentTemplate => deploymentTemplate.Id.Equals(id));

            var cursor = await collection.FindAsync(filter);

            var dp = await cursor.FirstOrDefaultAsync();

            return _protector.UnprotectObject(dp);
        }

        public async Task<DeploymentTemplate> CreateDeploymentTemplateAsync(string name, ApplicationType applicationType)
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate(name, applicationType);
            var protectedDeploymentTemplate = _protector.ProtectObject(deploymentTemplate);
            var collection = GetCollection<DeploymentTemplate>(DeploymentTemplateDao.CollectionName);
            
            await collection.InsertOneAsync(protectedDeploymentTemplate);

            deploymentTemplate.Id = protectedDeploymentTemplate.Id;
            return deploymentTemplate;
        }

        public async Task<DeploymentOrder> CreateDeploymentOrderAsync(string requestId, ApplicationType applicationType)
        {
            await CreateApplicationAsync("application", applicationType);
            await CreateDeploymentTemplateAsync("template", applicationType);

            var deploymentOrder = ModelFixture.BuildDeploymentOrder(applicationType);
            deploymentOrder.RequestId = requestId;

            var collection = GetCollection<DeploymentOrder>(DeploymentOrderDao.CollectionName);

            var protectedDeploymentOrder = _protector.ProtectObject(deploymentOrder);
            await collection.InsertOneAsync(protectedDeploymentOrder);

            deploymentOrder.Id = protectedDeploymentOrder.Id;
            return deploymentOrder;
        }

        public async Task<DeploymentOrder> GetDeploymentOrderByIdAsync(string id)
        {
            var collection = GetCollection<DeploymentOrder>(DeploymentOrderDao.CollectionName);

            var filter = new FilterDefinitionBuilder<DeploymentOrder>()
                .Where(deploymentOrder => deploymentOrder.Id.Equals(new ObjectId(id)));

            var cursor = await collection.FindAsync(filter);
            var dp = await cursor.FirstOrDefaultAsync();

            return _protector.UnprotectObject(dp);
        }
    }
}