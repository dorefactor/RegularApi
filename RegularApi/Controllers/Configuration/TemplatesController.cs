using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RegularApi.Domain.Views;

namespace RegularApi.Domain.Configuration
{
    [ApiController]
    public class TemplatesController : ConfigurationControllerBase
    {
        public async Task<IActionResult> NewAsync(TemplateView templateView)
        {


            return Ok("");
        }
    }
}