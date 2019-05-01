using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Domain.Views;
using RegularApi.Services;
using RegularApi.Transformers;

namespace RegularApi.Controllers.Configuration
{
    [ApiController]
    public class DeploymentTemplatesController : ConfigurationControllerBase
    {
        private readonly ILogger<DeploymentTemplatesController> _logger;
        private readonly IDeploymentTemplateTransformer _transformer;
        private readonly DeploymentTemplateService _deploymentTemplateService;

        public DeploymentTemplatesController(ILoggerFactory loggerFactory, IDeploymentTemplateTransformer transformer, DeploymentTemplateService deploymentTemplateService)
        {
            _logger = loggerFactory.CreateLogger<DeploymentTemplatesController>();
            _transformer = transformer;
            _deploymentTemplateService = deploymentTemplateService;
        }
        
        public async Task<IActionResult> NewAsync(DeploymentTemplateView deploymentTemplateView)
        {
            _logger.LogInformation("New deployment template request received: {0}", deploymentTemplateView);
            var template = _transformer.FromResource(deploymentTemplateView);

            var result = await _deploymentTemplateService.AddDeploymentTemplateAsync(template);

            return result.Match<IActionResult>(
                right => Ok(BuildNewResourceResponseView("/configuration/deploymentTemplates", right.Name)),
                left => UnprocessableEntity(BuildErrorResponse(left))
            );
        }
    }
}