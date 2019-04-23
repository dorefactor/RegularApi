using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Controllers.Dashboard.Models;
using RegularApi.Services;
using RegularApi.Transformers;

namespace RegularApi.Controllers.Dashboard
{
    [ApiController]
    [Route("/[controller]")]
    public class ApplicationController : AbstractController
    {
        private readonly ILogger<ApplicationController> _logger;
        private readonly ApplicationSetupService _applicationSetupService;
        private readonly IApplicationTransformer _applicationTransformer;

        public ApplicationController(ILoggerFactory loggerFactory,
                                     ApplicationSetupService applicationSetupService,
                                     IApplicationTransformer applicationTransformer)
        {
            _logger = loggerFactory.CreateLogger<ApplicationController>();
            _applicationSetupService = applicationSetupService;
            _applicationTransformer = applicationTransformer;
        }

        [HttpPost]
        public async Task<IActionResult> NewApplicationSetupAsync([FromBody] ApplicationResource applicationResource)
        {
            _logger.LogInformation("application setup request received: {0}", applicationResource);

            var application = _applicationTransformer.fromResource(applicationResource);

            var resultHolder = await _applicationSetupService.SaveApplicationSetupAsync(application);

            return resultHolder.Match<IActionResult>(
                right => Ok(),
                left => UnprocessableEntity(BuildErrorResponse(left))
            );
        }
    }
}