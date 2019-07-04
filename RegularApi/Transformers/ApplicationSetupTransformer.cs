using System;
using RegularApi.Domain.Model;
using RegularApi.Domain.Model.Docker;
using RegularApi.Domain.Views;
using RegularApi.Domain.Views.Docker;
using RegularApi.Enums;

namespace RegularApi.Transformers
{
    public class ApplicationSetupTransformer : ITransformer<ApplicationSetupView, ApplicationSetup>
    {
        public ApplicationSetup Transform(ApplicationSetupView applicationSetupView)
        {
            var applicationType = (ApplicationType)Enum.Parse(typeof(ApplicationType), applicationSetupView.Type);

            switch (applicationType)
            {
                case ApplicationType.Docker:
                    {
                        var dockerApplicationSetupView = (DockerApplicationSetupView)applicationSetupView;

                        var dockerApplicationSetup = new DockerApplicationSetup
                        {
                            ApplicationType = applicationType,
                            Ports = dockerApplicationSetupView.Ports,
                            EnvironmentVariables = dockerApplicationSetupView.EnvironmentVariables,
                            ExtraHosts = dockerApplicationSetupView.ExtraHosts,
                            Volumes = dockerApplicationSetupView.Volumes
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
                case ApplicationType.Docker:
                    {
                        var dockerApplicationSetup = (DockerApplicationSetup)applicationSetup;

                        var dockerApplicationSetupView = new DockerApplicationSetupView
                        {
                            Type = ApplicationType.Docker.ToString(),
                            Ports = dockerApplicationSetup.Ports,
                            EnvironmentVariables = dockerApplicationSetup.EnvironmentVariables,
                            ExtraHosts = dockerApplicationSetup.ExtraHosts,
                            Volumes = dockerApplicationSetup.Volumes
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
