using System;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RegularApi.Dao;
using RegularApi.RabbitMq.Templates;
using RegularApi.Domain.Services;

namespace RegularApi.Domain
{
    public class DeploymentService
    {
        private readonly ILogger<DeploymentService> _logger;
        private readonly IApplicationDao _applicationDao;
        private readonly IRabbitMqTemplate _rabbitMqTemplate;

        public DeploymentService(ILoggerFactory loggerFactory, IApplicationDao applicationDao, IRabbitMqTemplate rabbitMqTemplate)
        {
            _logger = loggerFactory.CreateLogger<DeploymentService>();
            _applicationDao = applicationDao;
            _rabbitMqTemplate = rabbitMqTemplate;
        }
        
        public async Task<Either<string, DeploymentRequest>> QueueDeploymentRequestAsync(string appName, string tag)
        {
            try
            {
                _logger.LogInformation("Deployment request for app: {0} tag: {1}", appName, tag);
                var applicationHolder = await _applicationDao.GetApplicationByNameAsync(appName);

                if (applicationHolder.IsNone)
                {
                    _logger.LogError("No application found with name: {0}", appName);
                
                    return "No application found with name: " + appName;
                }

                var appRequest = BuildDeploymentRequest(appName, tag);
                var payload = JsonConvert.SerializeObject(appRequest);

                _logger.LogInformation("Queue deployment request: {0}", payload);
                _rabbitMqTemplate.SendMessage(payload);
            
                return appRequest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "can't queue deployment request for app: {0}", appName);
                return "Can't queue deployment request for app: " + appName;
            }
        }

        private static DeploymentRequest BuildDeploymentRequest(string appName, string tag)
        {
            return new DeploymentRequest
            {
                RequestId = CreateRequestId(),
                Name = appName,
                Tag = tag,
                Created = DateTime.UtcNow
            };
        }
        
        private static string CreateRequestId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}