using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.Domain.Model.Docker;
using RegularApi.Domain.Services;
using RegularApi.RabbitMq.Templates;

namespace RegularApi.Services
{
    public class DeploymentService
    {
        private readonly ILogger<DeploymentService> _logger;
        private readonly IDeploymentTemplateDao _deploymentTemplateDao;
        private readonly IDeploymentOrderDao _deploymentOrderDao;
        private readonly IRabbitMqTemplate _rabbitMqTemplate;

        public DeploymentService(ILoggerFactory loggerFactory,
                                 IDeploymentTemplateDao deploymentTemplateDao,
                                 IDeploymentOrderDao deploymentOrderDao,
                                 IRabbitMqTemplate rabbitMqTemplate)
        {
            _logger = loggerFactory.CreateLogger<DeploymentService>();
            _deploymentTemplateDao = deploymentTemplateDao;
            _deploymentOrderDao = deploymentOrderDao;
            _rabbitMqTemplate = rabbitMqTemplate;
        }

        public async Task<Either<string, DeploymentRequest>> QueueDeploymentOrderAsync(DeploymentOrder deploymentOrder)
        {
            try
            {
                _logger.LogInformation("Deployment request for template_id: {0}", deploymentOrder.DeploymentTemplateId);

                var deploymentTemplateHolder = await _deploymentTemplateDao.GetByIdAsync(deploymentOrder.DeploymentTemplateId);

                if (deploymentTemplateHolder.IsNone)
                {
                    return "No deployment template_id found: " + deploymentOrder.DeploymentTemplateId;
                }

                var deploymentOrderHolder = await _deploymentOrderDao.SaveAsync(deploymentOrder);

                var deploymentOrderRequest = BuildDeploymentRequest(deploymentOrder.RequestId);
                var payload = JsonConvert.SerializeObject(deploymentOrderRequest);

                _logger.LogInformation("Queue deployment request: {0}", payload);

                _rabbitMqTemplate.SendMessage(payload);

                return deploymentOrderRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't queue deployment request for template_id: {0}", deploymentOrder.DeploymentTemplateId);
                return "Can't queue deployment request for template_id: " + deploymentOrder.DeploymentTemplateId;
            }
        }

        public async Task<Either<string, DeploymentOrder>> GetDeploymentOrderByRequestIdAsync(string id)
        {

            var deploymentOrderVoHolder = await _deploymentOrderDao.GetDeploymentOrderVoByRequestIdAsync(id);

            if (deploymentOrderVoHolder.IsNone)
            {
                return "No deployment order found with request_id found: " + id;
            }

            var deploymentOrderVo = deploymentOrderVoHolder.FirstOrDefault();

            return new DeploymentOrder
            {
                Id = deploymentOrderVo.Id,
                RequestId = deploymentOrderVo.RequestId,
                ApplicationSetup = GetApplicationSetup(deploymentOrderVo),
                HostsSetup = GetHostsSetup(deploymentOrderVo)
            };
        }

        private ApplicationSetup GetApplicationSetup(DeploymentOrderVo deploymentOrderVo)
        {
            var applicationSetupFromApplication = deploymentOrderVo.ApplicationSetupFromApplication;

            switch (applicationSetupFromApplication.ApplicationType)
            {
                case Enums.ApplicationType.Docker:
                    {
                        var dockerApplicationSetupFromApplication = (DockerApplicationSetup)deploymentOrderVo.ApplicationSetupFromApplication;
                        var dockerApplicationSetupFromDeploymentTemplate = (DockerApplicationSetup)deploymentOrderVo.ApplicationSetupFromDeploymentTemplate;
                        var dockerApplicationSetupFromDeploymentOrder = (DockerApplicationSetup)deploymentOrderVo.ApplicationSetupFromDeploymentOrder;

                        return new DockerApplicationSetup
                        {
                            ApplicationType = Enums.ApplicationType.Docker,
                            Registry = dockerApplicationSetupFromApplication.Registry,
                            Image = new Image
                            {
                                Name = dockerApplicationSetupFromApplication.Image.Name,
                                Tag = dockerApplicationSetupFromDeploymentOrder.Image?.Tag
                            },
                            Ports = dockerApplicationSetupFromApplication.Ports,
                            EnvironmentVariables = dockerApplicationSetupFromDeploymentTemplate.EnvironmentVariables
                        };
                    }
                default:
                    return null;
            }
        }

        private IList<HostSetup> GetHostsSetup(DeploymentOrderVo deploymentOrderVo)
        {
            var hostsSetupFromDeploymentTemplate = deploymentOrderVo.HostsSetupFromDeploymentTemplate;
            var hostsSetupFromDeploymentOrder = deploymentOrderVo.HostsSetupFromDeploymentOrder;

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

        private DeploymentRequest BuildDeploymentRequest(string requestId)
        {
            return new DeploymentRequest
            {
                RequestId = requestId,
                Created = DateTime.UtcNow,
            };
        }
    }
}