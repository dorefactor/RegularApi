using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;
using RegularApi.Services;
using RegularApi.Transformers;

namespace RegularApi.Controllers.Deployment
{
    [ApiController]
    [Route("/[controller]")]
    public class DeploymentController : AbstractController
    {
        private readonly ILogger<DeploymentController> _logger;
        private readonly ITransformer<DeploymentOrderView, DeploymentOrder> _deploymentOrderTransformer;
        private readonly DeploymentService _deploymentService;

        public DeploymentController(ILoggerFactory loggerFactory,
                                    ITransformer<DeploymentOrderView, DeploymentOrder> deploymentOrderTransformer,
                                    DeploymentService deploymentService)
        {
            _logger = loggerFactory.CreateLogger<DeploymentController>();
            _deploymentOrderTransformer = deploymentOrderTransformer;
            _deploymentService = deploymentService;
        }

        [HttpPost]
        public async Task<IActionResult> NewDeploymentAsync(DeploymentOrderView deploymentOrderView)
        {
            _logger.LogInformation("deployment request received: {0} - {1}", deploymentOrderView.DeploymentTemplateId, deploymentOrderView.Version);

            var deploymentOrder = _deploymentOrderTransformer.Transform(deploymentOrderView);

            var result = await _deploymentService.QueueDeploymentRequestAsync(deploymentOrder);

            var action = result.Match<IActionResult>(
                right => Ok(right),
                left => UnprocessableEntity(BuildErrorResponse(left)));

            return action;
        }
    }
}