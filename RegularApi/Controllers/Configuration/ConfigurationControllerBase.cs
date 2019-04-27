using Microsoft.AspNetCore.Mvc;

namespace RegularApi.Domain.Configuration
{
    [Route("/configuration/[controller]")]
    public abstract class ConfigurationControllerBase : AbstractController
    {
        
    }
}