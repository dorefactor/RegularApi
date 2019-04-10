using System.Linq;
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
        private readonly ILogger<DeploymentController> _logger;

        public DeploymentController(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<DeploymentController>();
        }
        
        [HttpPost]
        public IActionResult NewDeployment([FromBody] ApplicationRequest request)
        {
            _logger.LogInformation("deployment request received: {0} - {1}", request.Name, request.Tag);

            if (!ModelState.IsValid)
            {
                var errors = from modelError in (from modelState in ModelState.Values
                        select modelState.Errors).ToList()
                    select modelError .ToList();                
                
                return BadRequest(errors);
            }

            return Ok();
        }
    }
}