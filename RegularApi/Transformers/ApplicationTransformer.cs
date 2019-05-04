using System;
using RegularApi.Domain.Model;
using RegularApi.Domain.Model.Docker;
using RegularApi.Domain.Views;
using RegularApi.Domain.Views.Docker;

namespace RegularApi.Transformers
{
    public class ApplicationTransformer : ITransformer<ApplicationView, Application>
    {
        public Application Transform(ApplicationView applicationView)
        {
            return new Application
            {
                Name = applicationView.Name,
                ApplicationSetup = GetApplicationSetup(applicationView)
            };
        }

        public ApplicationView Transform(Application view)
        {
            throw new NotImplementedException();
        }

        private ApplicationSetup GetApplicationSetup(ApplicationView applicationView)
        {
            var applicationType = (Enums.ApplicationType)Enum.Parse(typeof(Enums.ApplicationType), applicationView.ApplicationSetupView.Type);

            switch (applicationType)
            {
                case Enums.ApplicationType.Docker:
                    var dockerSetupView = (DockerSetupView)applicationView.ApplicationSetupView;
                    return new DockerSetup
                    {
                        ApplicationType = applicationType,
                        Registry = new Registry
                        {
                            IsPrivate = dockerSetupView.RegistryView.IsPrivate,
                            Url = dockerSetupView.RegistryView.Url,
                            Username = dockerSetupView.RegistryView.Username,
                            Password = dockerSetupView.RegistryView.Password
                        },
                        Image = new Image
                        {
                            Name = dockerSetupView.ImageView.Name
                        },
                        ContainerName = applicationView.Name,
                        Ports = dockerSetupView.Ports,
                        EnvironmentVariables = dockerSetupView.EnvironmentVariables
                    };
                default:
                    return null;
            }
        }
    }
}
