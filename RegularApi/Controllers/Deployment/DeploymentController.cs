using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Controllers.Deployment.Views;

namespace RegularApi.Controllers.Deployment
{
    [ApiController]
    [Route("/[controller]")]
    public class DeploymentController : AbstractController
    {
        private readonly ILogger<DeploymentController> _logger;

        public DeploymentController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DeploymentController>();
        }
        
        [HttpPost]
        public IActionResult NewDeployment(ApplicationRequest request)
        {
            _logger.LogInformation("deployment request received: {0} - {1}", request.Name, request.Tag);

            return Ok();
        }
    }
}