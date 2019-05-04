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
    [Route("/deployment-orders")]
    public class DeploymentOrderController : AbstractController
    {
        private readonly ILogger<DeploymentOrderController> _logger;
        private readonly ITransformer<DeploymentOrderRequestView, DeploymentOrder> _deploymentOrderTransformer;
        private readonly DeploymentService _deploymentService;

        public DeploymentOrderController(ILoggerFactory loggerFactory,
                                         ITransformer<DeploymentOrderRequestView, DeploymentOrder> deploymentOrderTransformer,
                                         DeploymentService deploymentService)
        {
            _logger = loggerFactory.CreateLogger<DeploymentOrderController>();
            _deploymentOrderTransformer = deploymentOrderTransformer;
            _deploymentService = deploymentService;
        }

        [HttpPost]
        public async Task<IActionResult> NewAsync(DeploymentOrderRequestView deploymentOrderRequestView)
        {
            _logger.LogInformation("deployment request received: {0} - {1}", deploymentOrderRequestView.DeploymentTemplateId, deploymentOrderRequestView.Version);

            var deploymentOrder = _deploymentOrderTransformer.Transform(deploymentOrderRequestView);

            var result = await _deploymentService.QueueDeploymentOrderAsync(deploymentOrder);

            var action = result.Match<IActionResult>(
                right => Ok(right),
                left => UnprocessableEntity(BuildErrorResponse(left)));

            return action;
        }

        [HttpGet("{id}/summarized")]
        public async Task<IActionResult> GetDeploymentOrderByRequestIdAsync(string id)
        {
            var deploymentOrderSummarizedHolder = await _deploymentService.GetDeploymentOrderSummarizedByRequestIdAsync(id);

            return deploymentOrderSummarizedHolder.Match<IActionResult>(
                left => UnprocessableEntity(left),
                right => Ok(right));
        }
    }
}