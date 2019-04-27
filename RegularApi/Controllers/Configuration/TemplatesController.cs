using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegularApi.Domain.Views;

namespace RegularApi.Controllers.Configuration
{
    [ApiController]
    public class TemplatesController : ConfigurationControllerBase
    {
        public async Task<IActionResult> NewAsync(DeploymentTemplateView deploymentTemplateView)
        {


            return Ok("");
        }
    }
}