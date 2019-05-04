using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.Domain.Services;
using RegularApi.Domain.Views.Jenkins;
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

        public async Task<Either<string, DeploymentRequest>> QueueDeploymentRequestAsync(DeploymentOrder deploymentOrder)
        {
            try
            {
                _logger.LogInformation("Deployment request for template_id: {0} version: {1}", deploymentOrder.DeploymentTemplateId, deploymentOrder.ApplicationVersion);
                var deploymentTemplateHolder = await _deploymentTemplateDao.GetByIdAsync(deploymentOrder.DeploymentTemplateId);

                if (deploymentTemplateHolder.IsNone)
                {
                    return "No deployment template_id found: " + deploymentOrder.DeploymentTemplateId;
                }

                var deploymentOrderHolder = await _deploymentOrderDao.Save(deploymentOrder);

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

        public async Task<Either<string, JenkinsDeploymentOrder>> GetByRequestIdAsync(string id)
        {

            var deploymentOrderDetailHolder = await _deploymentOrderDao.GetDeploymentOrderDetailByRequestIdAsync(id);

            var deploymentOrderDetail = deploymentOrderDetailHolder.FirstOrDefault();

            var ansibleHosts = new List<AnsibleHost>();
            foreach (HostSetup hostSetup in deploymentOrderDetail.HostsSetup)
            {
                foreach (Host host in hostSetup.Hosts)
                {
                    ansibleHosts.Add(new AnsibleHost
                    {
                        PublicIp = host.Ip,
                        Variables = new Dictionary<string, string>()
                        {
                            { "ansible_ssh_user", host.Username },
                            { "ansible_user", host.Username },
                            { "ansible_password", host.Password }
                        }
                    });
                }
            }

            var ports = (from port in deploymentOrderDetail.DockerSetup.Ports
                         select new
                         {
                             PortMapping = port.Key + ":" + port.Value

                         }).ToList();


            var environmentVariables = (from environmentVariable in deploymentOrderDetail.DockerSetup.EnvironmentVariables
                                        select new
                                        {
                                            EnvironmentVariableMapping = environmentVariable.Key + ":" + environmentVariable.Value

                                        }).ToList();

            var ansibleGroup = new AnsibleGroup
            {
                AnsibleHosts = ansibleHosts,
                ApplicationSetup = new ApplicationSetup
                {
                    Name = deploymentOrderDetail.ApplicationName,
                    DockerSetup = new Domain.Views.Jenkins.DockerSetup
                    {
                        Image = deploymentOrderDetail.DockerSetup.ImageName + ":" + deploymentOrderDetail.ApplicationVersion,
                        Ports = ports.Select(portMapping => portMapping.PortMapping).ToList(),
                        EnvironmentVariables = environmentVariables.Select(environmentVariablesMapping => environmentVariablesMapping.EnvironmentVariableMapping).ToList()
                    }
                }
            };

            var jenkinsDeploymentOrder = new JenkinsDeploymentOrder
            {
                AnsibleSetup = new AnsibleSetup
                {
                    AnsibleGroups = new List<AnsibleGroup>() { ansibleGroup }
                }
            };


            var settings = new JsonSerializerSettings();
            settings.Formatting = Formatting.Indented;

            Console.WriteLine(JsonConvert.SerializeObject(jenkinsDeploymentOrder, settings));

            return jenkinsDeploymentOrder;

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