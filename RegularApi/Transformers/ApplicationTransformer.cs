using RegularApi.Domain.Views;
using RegularApi.Domain.Model;

namespace RegularApi.Transformers
{
    public class ApplicationTransformer : IApplicationTransformer
    {
        public Application fromResource(ApplicationView applicationView)
        {
            return new Application
            {
                Name = applicationView.Name,
                DockerSetup = new DockerSetup
                {
                    RegistryUrl = applicationView.DockerSetup.RegistryUrl,
                    ImageName = applicationView.DockerSetup.ImageName,
                    EnvironmentVariables = applicationView.DockerSetup.EnvironmentVariables,
                    Ports = applicationView.DockerSetup.Ports
                }
                // ,
                // HostsSetup = applicationResource.HostsSetupResources?.Select(hostsSetupResource => new HostSetup()
                // {
                //     TagName = hostsSetupResource.TagName,
                //     Hosts = hostsSetupResource.HostsResource?.Select(hostResource => new Host()
                //     {
                //         Ip = hostResource.Ip,
                //         Username = hostResource.Username,
                //         Password = hostResource.Password
                //     }).ToList()
                // }).ToList()
            };
        }
    }
}
