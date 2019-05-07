using System.Collections.Generic;
using MongoDB.Bson;
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
                            {"VARIABLE","VALUE"}
                        },
                    Ports = new Dictionary<string, string>
                        {
                            {"8080","80"}
                        }
                }
            };
        }

        public static DeploymentTemplateView BuildDeploymentTemplateView(string name, string type)
        {
            return new DeploymentTemplateView
            {
                Name = name,
                ApplicationId = "5cce4c0d0722ec669fe60fca",
                ApplicationSetupView = new DockerApplicationSetupView
                {
                    Type = type,
                    EnvironmentVariables = new Dictionary<string, string>
                        {
                            {"APP_NAME","todo-app"}
                        }
                },
                HostSetupViews = new List<HostSetupView>
                {
                    new HostSetupView
                    {
                        Tag = "QA",
                        HostViews = new List<HostView>
                        {
                            new HostView{
                                Ip = "192.168.99.1",
                                Username = "root",
                                Password = "****"
                            }
                        }
                    }
                }
            };
        }

        // public static NewResourceResponseView BuildDeploymentTemplateCreationResponse(string name)
        // {
        //     return new NewResourceResponseView
        //     {
        //         Link = "/configuration/deploymentTemplates/" + name
        //     };
        // }

    }
}