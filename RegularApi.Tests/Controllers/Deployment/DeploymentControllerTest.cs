using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RegularApi.Controllers.Deployment.Views;
using RegularApi.Controllers.Views;
using static Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary;

namespace RegularApi.Tests.Controllers.Deployment
{
    public class DeploymentControllerTest : IntegrationTestBase
    {

        const string DeploymentUri = "https://localhost:5001/deployment";

        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public async Task TestValidationErrors()
        {
            var applicationRequest = new ApplicationRequest();

            var responseMessage = await performPostAsync<ApplicationRequest>(applicationRequest, DeploymentUri);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);            
        }

        [Test]
        public async Task TestSuccessRequest()
        {
            var applicationRequest = new ApplicationRequest
            {
                Tag = "1.1.1",
                Name = "SuperApplication"
            };

            var responseMessage = await performPostAsync<ApplicationRequest>(applicationRequest, DeploymentUri);

            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        }

        private async Task<HttpResponseMessage> performPostAsync<T>(T request, string uri)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await HttpClient.PostAsync(uri, content);

            return responseMessage;
        }
    }
}