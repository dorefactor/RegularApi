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
                        var dockerApplicationSetupView = (DockerApplicationSetupView)applicationSetupView;

                        var dockerApplicationSetup = new DockerApplicationSetup
                        {
                            ApplicationType = applicationType,
                            Ports = dockerApplicationSetupView.Ports,
                            EnvironmentVariables = dockerApplicationSetupView.EnvironmentVariables
                        };

                        // RegistryView
                        if (dockerApplicationSetupView.RegistryView != null)
                        {
                            dockerApplicationSetup.Registry = new Registry
                            {
                                IsPrivate = dockerApplicationSetupView.RegistryView.IsPrivate,
                                Url = dockerApplicationSetupView.RegistryView.Url,
                                Username = dockerApplicationSetupView.RegistryView.Username,
                                Password = dockerApplicationSetupView.RegistryView.Password
                            };
                        }

                        // ImageView
                        if (dockerApplicationSetupView.ImageView != null)
                        {
                            dockerApplicationSetup.Image = new Image
                            {
                                Name = dockerApplicationSetupView.ImageView.Name,
                                Tag = dockerApplicationSetupView.ImageView.Tag
                            };
                        }

                        return dockerApplicationSetup;
                    }

                default:
                    return null;
            }
        }

        public ApplicationSetupView Transform(ApplicationSetup applicationSetup)
        {
            switch (applicationSetup.ApplicationType)
            {
                case Enums.ApplicationType.Docker:
                    {
                        var dockerApplicationSetup = (DockerApplicationSetup)applicationSetup;

                        var dockerApplicationSetupView = new DockerApplicationSetupView
                        {
                            Type = Enums.ApplicationType.Docker.ToString(),
                            Ports = dockerApplicationSetup.Ports,
                            EnvironmentVariables = dockerApplicationSetup.EnvironmentVariables
                        };

                        // Registry
                        if (dockerApplicationSetup.Registry != null)
                        {
                            dockerApplicationSetupView.RegistryView = new RegistryView
                            {
                                IsPrivate = dockerApplicationSetup.Registry.IsPrivate,
                                Url = dockerApplicationSetup.Registry.Url,
                                Username = dockerApplicationSetup.Registry.Username,
                                Password = dockerApplicationSetup.Registry.Password
                            };
                        }

                        // Image
                        if (dockerApplicationSetup.Image != null)
                        {
                            dockerApplicationSetupView.ImageView = new ImageView
                            {
                                Name = dockerApplicationSetup.Image.Name,
                                Tag = dockerApplicationSetup.Image.Tag
                            };
                        }

                        return dockerApplicationSetupView;
                    }

                default:
                    return null;
            }
        }
    }
}
