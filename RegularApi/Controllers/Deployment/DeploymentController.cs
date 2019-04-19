using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Controllers.Deployment.Views;
using RegularApi.Services;
using RegularApi.Services.Domain;

namespace RegularApi.Controllers.Deployment
{
    [ApiController]
    [Route("/[controller]")]
    public class DeploymentController : AbstractController
    {
        private readonly ILogger<DeploymentController> _logger;
        private readonly DeploymentService _deploymentService;

        public DeploymentController(ILoggerFactory loggerFactory, DeploymentService deploymentService)
        {
            _logger = loggerFactory.CreateLogger<DeploymentController>();
            _deploymentService = deploymentService;
        }

        [HttpPost]
        public async Task<IActionResult> NewDeploymentAsync(ApplicationRequest request)
        {
            _logger.LogInformation("deployment request received: {0} - {1}", request.Name, request.Tag);

            var result = await _deploymentService.QueueDeploymentRequestAsync(request.Name, request.Tag);

            var action = result.Match<IActionResult>(
                right => Ok(BuildResponse(right)),
                left => UnprocessableEntity(BuildErrorResponse(left)));

            return action;
        }

        private static ApplicationResponse BuildResponse(DeploymentRequest deploymentRequest)
        {
            return new ApplicationResponse
            {
                DeploymentId = deploymentRequest.RequestId,
                Name = deploymentRequest.Name,
                Tag = deploymentRequest.Tag,
                Received = deploymentRequest.Created
            };
        }
    }
}