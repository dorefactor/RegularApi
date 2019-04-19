using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RegularApi.Controllers.Dashboard.Models;
using RegularApi.Dao.Model;
using RegularApi.Services;
using RegularApi.Services.Domain;

namespace RegularApi.Controllers.Dashboard
{
    [ApiController]
    [Route("/[controller]")]
    public class ApplicationController : AbstractController
    {
        private readonly ILogger<ApplicationController> _logger;
        private readonly ApplicationSetupService _applicationSetupService;

        public ApplicationController(ILoggerFactory loggerFactory, ApplicationSetupService applicationSetupService)
        {
            _logger = loggerFactory.CreateLogger<ApplicationController>();
            _applicationSetupService = applicationSetupService;
        }

        [HttpPost]
        public async Task<IActionResult> NewApplicationSetupAsync([FromBody] ApplicationResource applicationResource)
        {
            _logger.LogInformation("application setup request received: {0}", applicationResource);
            var application = new Application
            {
                Name = applicationResource.Name,
                DockerSetup = new DockerSetup
                {
                    RegistryUrl = applicationResource.DockerSetupResource.RegistryUrl,
                    ImageName = applicationResource.DockerSetupResource.ImageName,
                    EnvironmentVariables = applicationResource.DockerSetupResource.EnvironmentVariables,
                    Ports = applicationResource.DockerSetupResource.Ports
                },
                Hosts = applicationResource.HostResources.Select(hostsConfigurationResource => new Host()
                {
                    Ip = hostsConfigurationResource.Ip,
                    Username = hostsConfigurationResource.Username,
                    Password = hostsConfigurationResource.Password
                }).ToList()
            };

            var resultHolder = await _applicationSetupService.SaveApplicationSetupAsync(application);

            return resultHolder.Match<IActionResult>(
                right => Ok(),
                left => UnprocessableEntity(BuildErrorResponse(left))
            );
        }

        [HttpGet]
        public IActionResult GetSomething()
        {
            return Ok();
        }
    }
}