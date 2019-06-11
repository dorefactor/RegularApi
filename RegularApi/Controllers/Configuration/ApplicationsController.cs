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
    public class ApplicationsController : ConfigurationControllerBase
    {
        private readonly ILogger<ApplicationsController> _logger;
        private readonly ApplicationService _applicationSetupService;
        private readonly ITransformer<ApplicationView, Application> _applicationTransformer;

        public ApplicationsController(ILogger<ApplicationsController> logger,
                                      ApplicationService applicationSetupService,
                                      ITransformer<ApplicationView, Application> applicationTransformer)
        {
            _logger = logger;
            _applicationSetupService = applicationSetupService;
            _applicationTransformer = applicationTransformer;
        }

        [HttpPost]
        public async Task<IActionResult> NewAsync(ApplicationView applicationView)
        {
            _logger.LogInformation("application setup request received: {0}", applicationView);

            var application = _applicationTransformer.Transform(applicationView);

            var resultHolder = await _applicationSetupService.AddApplicationSetupAsync(application);

            return resultHolder.Match<IActionResult>(
                right => Ok(BuildNewResourceResponseView("/configuration/applications", right.Id.ToString())),
                left => UnprocessableEntity(BuildErrorResponse(left))
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _applicationSetupService.GetAllApplicationsAsync();

            return result.Match<IActionResult>(
                right =>
                {
                    var view = right.Select(application => _applicationTransformer.Transform(application)).ToList();

                    return Ok(view);
                },
                left => UnprocessableEntity(BuildErrorResponse(left)));
        }
    }
}