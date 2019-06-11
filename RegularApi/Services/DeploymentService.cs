using System;
using System.Linq;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.RabbitMq.Templates;

namespace RegularApi.Services
{
    public class DeploymentService
    {
        private readonly ILogger<DeploymentService> _logger;
        private readonly IDeploymentTemplateDao _deploymentTemplateDao;
        private readonly IDeploymentOrderDao _deploymentOrderDao;
        private readonly IRabbitMqTemplate _rabbitMqTemplate;

        public DeploymentService(ILogger<DeploymentService> logger,
                                 IDeploymentTemplateDao deploymentTemplateDao,
                                 IDeploymentOrderDao deploymentOrderDao,
                                 IRabbitMqTemplate rabbitMqTemplate)
        {
            _logger = logger;
            _deploymentTemplateDao = deploymentTemplateDao;
            _deploymentOrderDao = deploymentOrderDao;
            _rabbitMqTemplate = rabbitMqTemplate;
        }

        public async Task<Either<string, DeploymentOrder>> QueueDeploymentOrderAsync(DeploymentOrder deploymentOrder)
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

                var deploymentOrderQueued = BuildDeploymentOrderResponse(deploymentOrder);
                var deploymentOrderRequestId = deploymentOrderQueued.RequestId;

                _logger.LogInformation("Queue deployment request: {0}", deploymentOrderRequestId);

                _rabbitMqTemplate.SendMessage(deploymentOrderRequestId);

                return deploymentOrderQueued;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't queue deployment request for template_id: {0}", deploymentOrder.DeploymentTemplateId);
                return "Can't queue deployment request for template_id: " + deploymentOrder.DeploymentTemplateId;
            }
        }

        public async Task<Either<string, DeploymentOrder>> GetDeploymentOrderByRequestIdAsync(string id)
        {

            var deploymentOrderVoHolder = await _deploymentOrderDao.GetByRequestIdAsync(id);

            if (deploymentOrderVoHolder.IsNone)
            {
                return "No deployment order found with request_id found: " + id;
            }

            return deploymentOrderVoHolder.FirstOrDefault();
        }

        private DeploymentOrder BuildDeploymentOrderResponse(DeploymentOrder deploymentOrder)
        {
            return new DeploymentOrder
            {
                RequestId = deploymentOrder.RequestId,
                CreatedAt = deploymentOrder.CreatedAt
            };
        }
    }
}