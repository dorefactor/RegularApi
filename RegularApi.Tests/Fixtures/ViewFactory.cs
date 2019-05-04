using System.Collections.Generic;
using RegularApi.Domain.Views;

namespace RegularApi.Tests.Fixtures
{
    public static class ViewFactory
    {
        public static ApplicationView BuildApplicationView()
        {

            return new ApplicationView
            {
                Name = "test-app",
                DockerSetup = new DockerSetupView
                {
                    ImageName = "image-name",
                    RegistryUrl = "registry-url",
                    EnvironmentVariables = new[] { new KeyValuePair<object, object>("key", "value") },
                    Ports = new[] { new KeyValuePair<object, object>("8080", "80") }

                }
            };
        }

        public static DeploymentTemplateView BuildDeploymentTemplateView(string name)
        {
            return new DeploymentTemplateView
            {
                Name = name,
                ApplicationId = "123123123",
                EnvironmentVariables = new List<KeyValuePair<object, object>>
                {
                    new KeyValuePair<object, object>("VARIABLE", "VALUE")
                },
                HostsSetupViews = new HostSetupView
                {
                    TagName = "feature/awesome-thing",
                    Hosts = new List<HostView>
                    {
                        new HostView
                        {
                            Ip = "10.10.10.1",
                            Username = "username",
                            Password = "****"

                        }
                    }
                },
                Ports = new List<KeyValuePair<object, object>>
                {
                    new KeyValuePair<object, object>("8080", "80")
                }
            };
        }
        
    }
}