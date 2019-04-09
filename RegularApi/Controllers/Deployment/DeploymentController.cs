using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Controllers.Deployment.Validators;
using RegularApi.Controllers.Deployment.Views;

namespace RegularApi.Controllers.Deployment
{
    [ApiController]
    [Route("/[controller]")]
    public class DeploymentController : AbstractController
    {
        private readonly DeploymentRequestValidator _deploymentRequestValidator;
        private readonly ILogger<DeploymentController> _logger;

        public DeploymentController(DeploymentRequestValidator deploymentRequestValidator, 
            ILoggerFactory loggerFactory)
        {
            _deploymentRequestValidator = deploymentRequestValidator;
            _logger = loggerFactory.CreateLogger<DeploymentController>();
        }
        
        [HttpPost]
        public IActionResult NewDeployment([FromBody] ApplicationRequest request)
        {
            _logger.LogInformation("deployment request received: {0} - {1}", request.Name, request.Tag);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            return Ok();
        }
    }
}