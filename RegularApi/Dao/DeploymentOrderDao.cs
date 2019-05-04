using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using MongoDB.Driver;
using RegularApi.Domain.Model;

namespace RegularApi.Dao
{
    public class DeploymentOrderDao : BaseDao, IDeploymentOrderDao
    {
        public static readonly string DeploymentOrderCollectionName = "deploymentsOrders";

        private readonly IMongoCollection<DeploymentOrder> _collection;

        public DeploymentOrderDao(IMongoClient mongoClient, string databaseName)
            : base(mongoClient, databaseName, DeploymentOrderCollectionName)
        {
            _collection = GetCollection<DeploymentOrder>();
        }

        public async Task<Option<DeploymentOrder>> SaveAsync(DeploymentOrder deploymentOrder)
        {
            await _collection.InsertOneAsync(deploymentOrder);

            return OfNullable(deploymentOrder);
        }

        public async Task<Option<DeploymentOrderVo>> GetDeploymentOrderVoByRequestIdAsync(string id)
        {
            var deploymentTemplatesCollection = GetCollection<DeploymentTemplate>(DeploymentTemplateDao.DeploymentTemplateCollectionName);
            var applicationsCollection = GetCollection<Application>(ApplicationDao.ApplicationCollectionName);

            var query = (from deploymentOrder in _collection.AsQueryable()
                         join deploymentTemplate in deploymentTemplatesCollection.AsQueryable() on deploymentOrder.DeploymentTemplateId equals deploymentTemplate.Id
                         join application in applicationsCollection.AsQueryable() on deploymentTemplate.ApplicationId equals application.Id
                         where deploymentOrder.RequestId.Equals(id)
                         select new DeploymentOrderVo
                         {
                             ApplicationName = application.Name,
                             ApplicationVersion = deploymentOrder.ApplicationVersion,
                             DockerSetup = new DockerSetup
                             {
                                 ImageName = application.DockerSetup.ImageName,
                                 EnvironmentVariables = deploymentTemplate.EnvironmentVariables,
                                 Ports = application.DockerSetup.Ports
                             },
                             HostsSetup = deploymentOrder.HostsSetup
                         }).ToAsyncEnumerable();

            return OfNullable(await query.FirstOrDefault());
        }
    }
}
