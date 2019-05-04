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

        public async Task<Either<string, DeploymentRequest>> QueueDeploymentOrderAsync(DeploymentOrder deploymentOrder)
        {
            try
            {
                _logger.LogInformation("Deployment request for template_id: {0} version: {1}", deploymentOrder.DeploymentTemplateId, deploymentOrder.ApplicationVersion);

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

        public async Task<Either<string, DeploymentOrderSummarized>> GetDeploymentOrderSummarizedByRequestIdAsync(string id)
        {

            var deploymentOrderVoHolder = await _deploymentOrderDao.GetDeploymentOrderVoByRequestIdAsync(id);

            if (deploymentOrderVoHolder.IsNone)
            {
                return "No deployment order found with request_id found: " + id;
            }

            var deploymentOrderVo = deploymentOrderVoHolder.FirstOrDefault();
            var deploymentOrderSummarized = new DeploymentOrderSummarized();
            //var deploymentOrderSummarized = new DeploymentOrderSummarized
            //{
            //AnsibleSetup = new AnsibleSetup()
            //AnsibleSetup = new AnsibleSetup
            //{
            //    AnsibleGroups = new List<AnsibleGroup>() { BuildAnsibleGroup(deploymentOrderVo) }
            //}
            //};

            //var settings = new JsonSerializerSettings();
            //settings.Formatting = Formatting.Indented;

            //Console.WriteLine(JsonConvert.SerializeObject(deploymentOrderSummarized, settings));

            return deploymentOrderSummarized;

        }

        private DeploymentRequest BuildDeploymentRequest(string requestId)
        {
            return new DeploymentRequest
            {
                RequestId = requestId,
                Created = DateTime.UtcNow,
            };
        }

        private List<AnsibleHost> GetAnsibleHosts(DeploymentOrderVo deploymentOrderVo)
        {
            var ansibleHosts = new List<AnsibleHost>();

            foreach (HostSetup hostSetup in deploymentOrderVo.HostsSetup)
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

            return ansibleHosts;
        }

        //private IList<string> BuildPorts(DeploymentOrderVo deploymentOrderVo)
        //{
        //    return (IList<string>)(from port in deploymentOrderVo.DockerSetup.Ports
        //                           select new string
        //                           (
        //                               port.Key + ":" + port.Value

        //                           )).ToList();
        //}

        //private IList<string> BuildEnvironmentVariables(DeploymentOrderVo deploymentOrderVo)
        //{
        //    return (IList<string>)(from environmentVariable in deploymentOrderVo.DockerSetup.EnvironmentVariables
        //                           select new string
        //                           (
        //                               environmentVariable.Key + ": " + environmentVariable.Value

        //                           )).ToList();
        //}

        //public AnsibleGroup BuildAnsibleGroup(DeploymentOrderVo deploymentOrderVo)
        //{
        //    return new AnsibleGroup
        //    {
        //        AnsibleHosts = GetAnsibleHosts(deploymentOrderVo),
        //        ApplicationSetup = new ApplicationSetup
        //        {
        //            Name = deploymentOrderVo.ApplicationName,
        //            DockerSetup = new Domain.Views.Jenkins.DockerSetup
        //            {
        //                Image = deploymentOrderVo.DockerSetup.ImageName + ":" + deploymentOrderVo.ApplicationVersion,
        //                Ports = BuildPorts(deploymentOrderVo).Select(portMapping => portMapping).ToList(),
        //                EnvironmentVariables = BuildEnvironmentVariables(deploymentOrderVo).Select(environmentVariablesMapping => environmentVariablesMapping).ToList()
        //            }
        //        }
        //    };
        //}

    }
}