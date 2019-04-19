using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using RegularApi.Controllers.Dashboard.Models;

namespace RegularApi.Tests.Controllers.Dashboard
{
    public class ApplicationControllerIT : BaseControllerIT
    {
        private const string APPLICATION_URI = "http://192.168.99.1:5000/application";
        //private const string APPLICATION_URI = "/application";

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
            //var response = await GetResponse<IActionResult>(responseMessage);

            // await DeleteApplication(applicationR.Id);

            responseMessage = await PerformGetAsync(APPLICATION_URI);
            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

            // Assert.NotNull(response.DeploymentId);
            // Assert.NotNull(response.Received);
            // Assert.AreEqual(applicationRequest.Name, response.Name);
            // Assert.AreEqual(applicationRequest.Tag, response.Tag);
        }

        // ------------------------------------------------------------------------------------------------

        private async Task<HttpResponseMessage> PerformPostAsync<T>(T request, string uri)
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.Indented
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await HttpClient.PostAsync(uri, content);

            return responseMessage;
        }

        private async Task<HttpResponseMessage> PerformGetAsync(string uri)
        {
            var responseMessage = HttpClient.GetAsync(uri);

            return await responseMessage;
        }


        private async Task<T> GetResponse<T>(HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}