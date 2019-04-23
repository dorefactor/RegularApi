using System.Linq;
using RegularApi.Controllers.Dashboard.Models;
using RegularApi.Dao.Model;

namespace RegularApi.Transformers
{
    public class ApplicationTransformer : IApplicationTransformer
    {
        public Application fromResource(ApplicationResource applicationResource)
        {
            return new Application
            {
                Name = applicationResource.Name,
                DockerSetup = new DockerSetup
                {
                    RegistryUrl = applicationResource.DockerSetupResource.RegistryUrl,
                    ImageName = applicationResource.DockerSetupResource.ImageName,
                    EnvironmentVariables = applicationResource.DockerSetupResource.EnvironmentVariables,
                    Ports = applicationResource.DockerSetupResource.Ports
                },
                HostsSetup = applicationResource.HostsSetupResources?.Select(hostsSetupResource => new HostSetup()
                {
                    TagName = hostsSetupResource.TagName,
                    Hosts = hostsSetupResource.HostsResource?.Select(hostResource => new Host()
                    {
                        Ip = hostResource.Ip,
                        Username = hostResource.Username,
                        Password = hostResource.Password
                    }).ToList()
                }).ToList()
            };
        }
    }
}
