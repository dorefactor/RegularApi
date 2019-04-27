using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RegularApi.Domain.Views;

namespace RegularApi.Tests.Controllers.Configuration
{
    public class ApplicationsControllerIT : BaseControllerIT
    {
        private const string ApplicationUri = "/configuration/applications";

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public async Task TestNewApplicationSetupAsync_Created()
        {
            var applicationResource = new ApplicationView 
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

            var responseMessage = await PerformPostAsync(applicationResource, ApplicationUri);

            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        }
    }
}