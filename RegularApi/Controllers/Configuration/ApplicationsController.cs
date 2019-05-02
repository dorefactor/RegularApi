using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Domain.Views;
using RegularApi.Services;
using RegularApi.Transformers;

namespace RegularApi.Controllers.Configuration
{
    [ApiController]
    public class ApplicationsController : ConfigurationControllerBase
    {
        private readonly ILogger<ApplicationsController> _logger;
        private readonly ApplicationSetupService _applicationSetupService;
        private readonly IApplicationTransformer _applicationTransformer;

        public ApplicationsController(ILoggerFactory loggerFactory,
                                     ApplicationSetupService applicationSetupService,
                                     IApplicationTransformer applicationTransformer)
        {
            _logger = loggerFactory.CreateLogger<ApplicationsController>();
            _applicationSetupService = applicationSetupService;
            _applicationTransformer = applicationTransformer;
        }

        [HttpPost]
        public async Task<IActionResult> NewApplicationSetupAsync(ApplicationView applicationView)
        {
            _logger.LogInformation("application setup request received: {0}", applicationView);

            var application = _applicationTransformer.FromView(applicationView);

            var resultHolder = await _applicationSetupService.SaveApplicationSetupAsync(application);

            return resultHolder.Match<IActionResult>(
                right => Ok(),
                left => UnprocessableEntity(BuildErrorResponse(left))
            );
        }
    }
}