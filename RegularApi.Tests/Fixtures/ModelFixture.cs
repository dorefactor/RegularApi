using System.Collections.Generic;
using MongoDB.Bson;
using RegularApi.Domain.Model;
using RegularApi.Domain.Model.Docker;
using RegularApi.Enums;

namespace RegularApi.Tests.Fixtures
{
    public static class ModelFixture
    {
        public static Application BuildApplication(string name, ApplicationType applicationType)
        {
            return new Application
            {
                Id = new ObjectId("5cce4c0d0722ec669fe60fca"),
                Name = name,
                ApplicationSetup = new DockerApplicationSetup
                {
                    ApplicationType = applicationType,
                    Registry = new Registry
                    {
                        IsPrivate = false,
                        Url = "https://registry.docker.com"
                    },
                    Image = new Image
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

        public static DeploymentTemplate BuildDeploymentTemplate(string name, ApplicationType applicationType)
        {
            return new DeploymentTemplate
            {
                Id = new ObjectId("5cce4c0d0722ec669fe60fcb"),
                Name = name,
                ApplicationId = new ObjectId("5cce4c0d0722ec669fe60fca"),
                ApplicationSetup = new DockerApplicationSetup
                {
                    ApplicationType = applicationType,
                    EnvironmentVariables = new Dictionary<string, string>
                        {
                            {"APP_NAME","todo-app"}
                        }
                },
                HostsSetup = new List<HostSetup>
                {
                    new HostSetup
                    {
                        Tag = "QA",
                        Hosts = new List<Host>
                        {
                            new Host{
                                Ip = "192.168.99.1",
                                Username = "root",
                                Password = "r00t"
                            }
                        }
                    }
                }
            };
        }

        public static DeploymentOrder BuildDeploymentOrder(ApplicationType applicationType)
        {
            return new DeploymentOrder
            {
                Id = new ObjectId("5cce4c260722ec669fe60fcc"),
                DeploymentTemplateId = new ObjectId("5cce4c0d0722ec669fe60fcb"),
                ApplicationSetup = new DockerApplicationSetup
                {
                    ApplicationType = applicationType,
                    Image = new Image
                    {
                        Tag = "1.0"
                    }
                },
                HostsSetup = new List<HostSetup>
                {
                    new HostSetup
                    {
                        Tag = "QA",
                        Hosts = new List<Host>
                        {
                            new Host{
                                Ip = "192.168.99.1"
                            }
                        }
                    }
                }
            };
        }
    }
}