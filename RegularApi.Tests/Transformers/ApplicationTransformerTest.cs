using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RegularApi.Controllers.Configuration.Models;
using RegularApi.Transformers;

namespace RegularApi.Tests.Transformers
{
    public class ApplicationTransformerTest
    {
        private IApplicationTransformer _applicationTransformer;

        [SetUp]
        public void SetUp()
        {
            _applicationTransformer = new ApplicationTransformer();
        }

        [Test]
        public void TestTransformFromResource()
        {
            var applicationResource = buildApplicationResource();
            var application = _applicationTransformer.fromResource(applicationResource);

            application.Name.Should().BeSameAs(applicationResource.Name);
            application.DockerSetup.Should().BeEquivalentTo(applicationResource.DockerSetupResource);
            // application.HostsSetup[0].TagName.Should().BeEquivalentTo(applicationResource.HostsSetupResources[0].TagName);
            // application.HostsSetup[0].Hosts[0].Should().BeEquivalentTo(applicationResource.HostsSetupResources[0].HostsResource[0]);
        }

        private ApplicationResource buildApplicationResource()
        {

            return new ApplicationResource()
            {
                Name = "test-app",
                DockerSetupResource = new DockerSetupResource()
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
