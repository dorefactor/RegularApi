using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Controllers.Deployment.Validators;
using RegularApi.Controllers.Deployment.Views;

namespace RegularApi.Controllers.Deployment
{
    [ApiController]
    [Route("/[controller]")]
    public class DeploymentsController : ControllerBase
    {
        private readonly DeploymentRequestValidator _deploymentRequestValidator;
        private readonly ILogger<DeploymentsController> _logger;

        public DeploymentsController(DeploymentRequestValidator deploymentRequestValidator, 
            ILoggerFactory loggerFactory)
        {
            _deploymentRequestValidator = deploymentRequestValidator;
            _logger = loggerFactory.CreateLogger<DeploymentsController>();
        }
        
        [HttpPost]
        public IActionResult NewDeployment([FromBody] ApplicationRequest request)
        {
            _logger.LogInformation("deployment request received: {0} - {1}", request.Name, request.Tag);
            var errors = _deploymentRequestValidator.Validate(request);

            if (errors.Count > 0)
            {
                return BadRequest();
            }
                       
            return Ok();
        }
    }
}