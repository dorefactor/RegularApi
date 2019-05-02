using Microsoft.AspNetCore.Mvc;

namespace RegularApi.Controllers.Configuration
{
    [Route("/configuration/[controller]")]
    public abstract class ConfigurationControllerBase : AbstractController
    {        
    }
}