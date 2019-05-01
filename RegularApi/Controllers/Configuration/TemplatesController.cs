using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Domain.Views;
using RegularApi.Services;
using RegularApi.Transformers;

namespace RegularApi.Controllers.Configuration
{
    [ApiController]
    public class TemplatesController : ConfigurationControllerBase
    {
        private readonly ILogger<TemplatesController> _logger;
        private readonly IDeploymentTemplateTransformer _transformer;
        private readonly DeploymentTemplateService _deploymentTemplateService;

        public TemplatesController(ILoggerFactory loggerFactory, IDeploymentTemplateTransformer transformer, DeploymentTemplateService deploymentTemplateService)
        {
            _logger = loggerFactory.CreateLogger<TemplatesController>();
            _transformer = transformer;
            _deploymentTemplateService = deploymentTemplateService;
        }
        
        public async Task<IActionResult> NewAsync(DeploymentTemplateView deploymentTemplateView)
        {
            _logger.LogInformation("New deployment template request received: {0}", deploymentTemplateView);
            var template = _transformer.FromResource(deploymentTemplateView);

            var result = await _deploymentTemplateService.AddDeploymentTemplateAsync(template);

            return result.Match<IActionResult>(
                right => Ok(BuildNewResourceResponseView("/configuration/templates", right.Name)),
                left => UnprocessableEntity(BuildErrorResponse(left))
            );
        }
    }
}