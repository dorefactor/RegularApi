using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RegularApi.Controllers.Dashboard.Models;

namespace RegularApi.Tests.Controllers.Dashboard
{
    public class ApplicationControllerIT : BaseControllerIT
    {
        private const string APPLICATION_URI = "/application";

        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public async Task TestNewApplicationSetupAsync_Created()
        {
            var applicationResource = new ApplicationResource()
            {
                Name = "test-app",
                DockerSetupResource = new DockerSetupResource()
                {
                    ImageName = "image-name",
                    RegistryUrl = "registry-url",
                    EnvironmentVariables = new[] { new KeyValuePair<object, object>("key", "value") },
                    Ports = new[] { new KeyValuePair<object, object>("8080", "80") }

                },
                HostResources = new HostResource[]
                {
                    new HostResource()
                    {
                        Ip = "192.168.99.1",
                        Username = "root",
                        Password = "r00t"
                    },
                }
            };

            var responseMessage = await PerformPostAsync(applicationResource, APPLICATION_URI);

            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        }
    }
}