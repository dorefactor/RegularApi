using RegularApi.Domain.Model;
using RegularApi.Domain.Views;

namespace RegularApi.Transformers
{
    public class ApplicationTransformer : ITransformer<ApplicationView, Application>
    {
        public Application Transform(ApplicationView applicationView)
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

        public ApplicationView Transform(Application view)
        {
            throw new System.NotImplementedException();
        }
    }
}
