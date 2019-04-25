using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RegularApi.Controllers.Configuration
{
    [ApiController]
    public class TemplatesController : ConfigurationControllerBase
    {
        public async Task<IActionResult> NewAsync()
        {

            return Ok("");
        }
    }
}