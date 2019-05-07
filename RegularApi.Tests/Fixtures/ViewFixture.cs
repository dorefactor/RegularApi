using System.Collections.Generic;
using RegularApi.Domain.Views;
using RegularApi.Domain.Views.Docker;

namespace RegularApi.Tests.Fixtures
{
    public static class ViewFixture
    {
        public static ApplicationView BuildApplicationView(string name, string type)
        {
            return new ApplicationView
            {
                Name = name,
                ApplicationSetupView = new DockerApplicationSetupView
                {
                    Type = type,
                    RegistryView = new RegistryView
                    {
                        IsPrivate = false,
                        Url = "https://registry.docker.com"
                    },
                    ImageView = new ImageView
                    {
                        Name = "todo-app"
                    },
                    EnvironmentVariables = new Dictionary<string, string>
                        {
                            {"APP_NAME",""}
                        },
                    Ports = new Dictionary<string, string>
                        {
                            {"8080","80"}
                        }
                }
            };
        }

        // public static DeploymentTemplateView BuildDeploymentTemplateView(string name)
        // {
        //     return new DeploymentTemplateView
        //     {
        //         Name = name,
        //         ApplicationId = "01234567890123456789ABCD",
        //         EnvironmentVariables = new List<KeyValuePair<object, object>>
        //         {
        //             new KeyValuePair<object, object>("VARIABLE", "VALUE")
        //         },
        //         HostsSetup = new List<HostSetupView>
        //         {
        //             new HostSetupView
        //             {
        //                 TagName = "feature/awesome-thing",
        //                 HostViews = new List<HostView>
        //                 {
        //                     new HostView
        //                     {
        //                         Ip = "10.10.10.1",
        //                         Username = "username",
        //                         Password = "****"

        //                     }
        //                 }
        //             }
        //         },
        //         Ports = new List<KeyValuePair<object, object>>
        //         {
        //             new KeyValuePair<object, object>("8080", "80")
        //         }
        //     };
        // }

        // public static NewResourceResponseView BuildDeploymentTemplateCreationResponse(string name)
        // {
        //     return new NewResourceResponseView
        //     {
        //         Link = "/configuration/deploymentTemplates/" + name
        //     };
        // }

    }
}