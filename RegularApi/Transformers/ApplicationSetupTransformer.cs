using System;
using RegularApi.Domain.Model;
using RegularApi.Domain.Model.Docker;
using RegularApi.Domain.Views;
using RegularApi.Domain.Views.Docker;

namespace RegularApi.Transformers
{
    public class ApplicationSetupTransformer : ITransformer<ApplicationSetupView, ApplicationSetup>
    {
        public ApplicationSetup Transform(ApplicationSetupView applicationSetupView)
        {
            var applicationType = (Enums.ApplicationType)Enum.Parse(typeof(Enums.ApplicationType), applicationSetupView.Type);

            switch (applicationType)
            {
                case Enums.ApplicationType.Docker:
                    {

                        var dockerSetupView = (DockerSetupView)applicationSetupView;

                        var dockerApplicationSetup = new DockerApplicationSetup
                        {
                            ApplicationType = applicationType,
                            Ports = dockerSetupView.Ports,
                            EnvironmentVariables = dockerSetupView.EnvironmentVariables
                        };

                        // RegistryView
                        if (dockerSetupView.RegistryView != null)
                        {
                            dockerApplicationSetup.Registry = new Registry
                            {
                                IsPrivate = dockerSetupView.RegistryView.IsPrivate,
                                Url = dockerSetupView.RegistryView.Url,
                                Username = dockerSetupView.RegistryView.Username,
                                Password = dockerSetupView.RegistryView.Password
                            };
                        }

                        // ImageView
                        if (dockerSetupView.ImageView != null)
                        {
                            dockerApplicationSetup.Image = new Image
                            {
                                Name = dockerSetupView.ImageView.Name,
                                Tag = dockerSetupView.ImageView.Tag
                            };
                        }

                        return dockerApplicationSetup;
                    }

                default:
                    return null;
            }
        }

        public ApplicationSetupView Transform(ApplicationSetup model)
        {
            throw new NotImplementedException();
        }
    }
}
