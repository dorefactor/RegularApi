using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class ApplicationTransformer : IApplicationTransformer
    {
        public Application FromResource(ApplicationView applicationView)
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
            };
        }
    }
}
