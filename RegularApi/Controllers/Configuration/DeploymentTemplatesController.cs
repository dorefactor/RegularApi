using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;
using RegularApi.Services;
using RegularApi.Transformers;

namespace RegularApi.Controllers.Configuration
{
    [ApiController]
    [Route("/configuration/deployment-templates")]
    public class DeploymentTemplatesController : ConfigurationControllerBase
    {
        private readonly ILogger<DeploymentTemplatesController> _logger;
        private readonly ITransformer<DeploymentTemplateView, DeploymentTemplate> _deploymentTemplateTransformer;
        private readonly DeploymentTemplateService _deploymentTemplateService;

        public DeploymentTemplatesController(ILogger<DeploymentTemplatesController> logger,
                                             ITransformer<DeploymentTemplateView, DeploymentTemplate> deploymentTemplateTransformer,
                                             DeploymentTemplateService deploymentTemplateService)
        {
            _logger = logger;
            _deploymentTemplateTransformer = deploymentTemplateTransformer;
            _deploymentTemplateService = deploymentTemplateService;
        }

        [HttpPost]
        public async Task<IActionResult> NewAsync(DeploymentTemplateView deploymentTemplateView)
        {
            _logger.LogInformation("New deployment template request received: {0}", deploymentTemplateView);
            
            var template = _deploymentTemplateTransformer.Transform(deploymentTemplateView);

            var result = await _deploymentTemplateService.AddDeploymentTemplateAsync(template);

            return result.Match<IActionResult>(
                right => Ok(BuildNewResourceResponseView("/configuration/deployment-templates", right.Name)),
                left => UnprocessableEntity(BuildErrorResponse(left))
            );
        }

        [HttpGet("{templateName}")]
        public async Task<IActionResult> GetAsync([FromRoute] string templateName)
        {
            _logger.LogInformation("Get deployment template: {0}", templateName);

            var result = await _deploymentTemplateService.GetDeploymentTemplateByNameAsync(templateName);

            return result.Match<IActionResult>(
                right =>
                {
                    var view = _deploymentTemplateTransformer.Transform(right);
                    return Ok(view);
                },
                left =>
                {
                    if (NotFoundResponse(templateName).Equals(left))
                    {
                        return NotFound();
                    }

                    return UnprocessableEntity(BuildErrorResponse(left));
                });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _deploymentTemplateService.GetAllDeploymentTemplatesAsync();

            return result.Match<IActionResult>(
                right =>
                {
                    var view = right.Select(deploymentTemplate => _deploymentTemplateTransformer.Transform(deploymentTemplate)).ToList();
                    return Ok(view);
                },
                left => UnprocessableEntity(BuildErrorResponse(left)));
        }

        private static string NotFoundResponse(string templateName)
        {
            return "Deployment template: " + templateName + " not found";
        }
    }
}