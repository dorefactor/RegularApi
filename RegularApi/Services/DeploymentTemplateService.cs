using System;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.Logging;
using RegularApi.Dao;
using RegularApi.Domain.Model;

namespace RegularApi.Services
{
    public class DeploymentTemplateService
    {
        private readonly ILogger<DeploymentTemplateService> _logger;
        private readonly IDeploymentTemplateDao _deploymentTemplateDao;

        public DeploymentTemplateService(ILogger<DeploymentTemplateService> logger, IDeploymentTemplateDao deploymentTemplateDao)
        {
            _logger = logger;
            _deploymentTemplateDao = deploymentTemplateDao;
        }

        public async Task<Either<string, DeploymentTemplate>> AddDeploymentTemplateAsync(DeploymentTemplate deploymentTemplate)
        {
            var deploymentTemplateName = deploymentTemplate.Name;

            try
            {
                _logger.LogInformation("creating new deployment template: {0}", deploymentTemplateName);
                var existingTemplateHolder = await _deploymentTemplateDao.GetByNameAsync(deploymentTemplate.Name);

                if (existingTemplateHolder.IsSome)
                {
                    _logger.LogError("deployment template: {0} already exists", deploymentTemplateName);
                    return "Deployment template: " + deploymentTemplateName + " already exists";
                }

                var template = await _deploymentTemplateDao.SaveAsync(deploymentTemplate);
                _logger.LogInformation("deployment template: {0} created successfully", deploymentTemplateName);

                return template;
            }
            catch (Exception ex)
            {
                _logger.LogError("can't create template: {0}", deploymentTemplate, ex);
                return "Can't create deployment template: " + deploymentTemplateName;
            }
        }

        public async Task<Either<string, DeploymentTemplate>> GetDeploymentTemplateByNameAsync(string name)
        {
            try
            {
                _logger.LogInformation("getting deployment template: {0} from DB", name);

                var templateHolder = await _deploymentTemplateDao.GetByNameAsync(name);

                if (templateHolder.IsNone)
                {
                    _logger.LogError("deployment template: {0} not found", name);
                    return "Deployment template: " + name + " not found";
                }

                _logger.LogInformation("deployment template: {0} found", name);
                return templateHolder.AsEnumerable().First();
            }
            catch (Exception ex)
            {
                _logger.LogError("can't get template: {0}", name, ex);
                return "Can't get deployment template: " + name;
            }
        }
    }
}