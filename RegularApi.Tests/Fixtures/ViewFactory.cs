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
                // ,
                // HostsSetupResources = new HostSetupResource[] {
                //     new HostSetupResource {
                //         TagName = "TAG",
                //         HostsResource = new HostResource[] {
                //             new HostResource
                //             {
                //                 Ip = "192.168.1.1",
                //                 Username = "root",
                //                 Password = "r00t"
                //             }
                //         }
                //     }
                // }
            };
        }
        
    }
}