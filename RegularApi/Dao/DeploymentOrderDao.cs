using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using MongoDB.Driver;
using RegularApi.Domain.Model;
using RegularApi.Domain.Model.Docker;

namespace RegularApi.Dao
{
    public class DeploymentOrderDao : BaseDao, IDeploymentOrderDao
    {
        public static readonly string CollectionName = "deploymentsOrders";

        private readonly IMongoCollection<DeploymentOrder> _collection;

        public DeploymentOrderDao(IMongoClient mongoClient, string databaseName)
            : base(mongoClient, databaseName, CollectionName)
        {
            _collection = GetCollection<DeploymentOrder>();
        }

        public async Task<DeploymentOrder> SaveAsync(DeploymentOrder deploymentOrder)
        {
            await _collection.InsertOneAsync(deploymentOrder);

            return deploymentOrder;
        }

        public async Task<Option<DeploymentOrder>> GetByRequestIdAsync(string id)
        {
            var deploymentTemplatesCollection = GetCollection<DeploymentTemplate>(DeploymentTemplateDao.CollectionName);
            var applicationsCollection = GetCollection<Application>(ApplicationDao.CollectionName);

            var query = (from deploymentOrder in _collection.AsQueryable()
                         join deploymentTemplate in deploymentTemplatesCollection.AsQueryable() on deploymentOrder.DeploymentTemplateId equals deploymentTemplate.Id
                         join application in applicationsCollection.AsQueryable() on deploymentTemplate.Application.Id equals application.Id
                         where deploymentOrder.RequestId.Equals(id)
                         select new
                         {
                             deploymentOrder.Id,
                             deploymentOrder.RequestId,
                             ApplicationFromApplication = application,
                             ApplicationFromDeploymentTemplate = deploymentTemplate.Application,
                             ApplicationFromDeploymentOrder = deploymentOrder.Application,
                             HostsSetupFromDeploymentTemplate = deploymentTemplate.HostsSetup,
                             HostsSetupFromDeploymentOrder = deploymentOrder.HostsSetup
                         }).ToAsyncEnumerable();

            var queryResult = await query.FirstOrDefault();

            return queryResult.IsNull() ? Option<DeploymentOrder>.None : Option<DeploymentOrder>.Some(new DeploymentOrder
            {
                Id = queryResult.Id,
                RequestId = queryResult.RequestId,
                Application = GetApplication(queryResult),
                HostsSetup = GetHostsSetup(queryResult)
            });
        }

        private Application GetApplication(dynamic queryResult)
        {
            var applicationFromApplication = queryResult.ApplicationFromApplication;

            switch (applicationFromApplication.ApplicationSetup.ApplicationType)
            {
                case Enums.ApplicationType.Docker:
                    {
                        var dockerApplicationSetupFromApplication = (DockerApplicationSetup)queryResult.ApplicationFromApplication.ApplicationSetup;
                        var dockerApplicationSetupFromDeploymentTemplate = (DockerApplicationSetup)queryResult.ApplicationFromDeploymentTemplate.ApplicationSetup;
                        var dockerApplicationSetupFromDeploymentOrder = (DockerApplicationSetup)queryResult.ApplicationFromDeploymentOrder.ApplicationSetup;

                        return new Application
                        {
                            Name = applicationFromApplication.Name,
                            ApplicationSetup = new DockerApplicationSetup
                            {
                                ApplicationType = Enums.ApplicationType.Docker,
                                Registry = dockerApplicationSetupFromApplication.Registry,
                                Image = new Image
                                {
                                    Name = dockerApplicationSetupFromApplication.Image?.Name,
                                    Tag = dockerApplicationSetupFromDeploymentOrder.Image?.Tag
                                },
                                Ports = dockerApplicationSetupFromApplication.Ports,
                                EnvironmentVariables = dockerApplicationSetupFromDeploymentTemplate.EnvironmentVariables
                            }
                        };
                    }
                default:
                    return null;
            }
        }

        private IList<HostSetup> GetHostsSetup(dynamic queryResult)
        {
            IList<HostSetup> hostsSetupFromDeploymentTemplate = queryResult.HostsSetupFromDeploymentTemplate;
            IList<HostSetup> hostsSetupFromDeploymentOrder = queryResult.HostsSetupFromDeploymentOrder;

            var hostsSetup = new List<HostSetup>();
            foreach (HostSetup hostSetupFromDeploymentOrder in hostsSetupFromDeploymentOrder)
            {
                var hostSetup = (from hostSetupFromDeploymentTemplate in hostsSetupFromDeploymentTemplate
                                 where hostSetupFromDeploymentTemplate.Tag.Equals(hostSetupFromDeploymentOrder.Tag)
                                 select new HostSetup
                                 {
                                     Tag = hostSetupFromDeploymentOrder.Tag,
                                     Hosts = (from hostFromDeploymentOrder in hostSetupFromDeploymentOrder.Hosts
                                              join hostFromDeploymentTemplate in hostSetupFromDeploymentTemplate.Hosts
                                                                                on hostFromDeploymentOrder.Ip equals hostFromDeploymentTemplate.Ip
                                              select new Host
                                              {
                                                  Ip = hostFromDeploymentTemplate.Ip,
                                                  Username = hostFromDeploymentTemplate.Username,
                                                  Password = hostFromDeploymentTemplate.Password
                                              }).ToList()

                                 }).SingleOrDefault();

                hostsSetup.Add(hostSetup);
            }

            return hostsSetup;
        }
    }
}
