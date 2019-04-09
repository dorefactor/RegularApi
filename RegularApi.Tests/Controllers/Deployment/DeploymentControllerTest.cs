using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using RegularApi.Controllers.Deployment.Views;
using RegularApi.Controllers.Views;

namespace RegularApi.Tests.Controllers.Deployment
{
    public class DeploymentControllerTest : IntegrationTestBase
    {
        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public async Task TestError()
        {
            var applicationRequest = new ApplicationRequest()
//            {
//                Name = "blah"
//                ,
//                Tag = "bleh"
//            }
                ;

            var json = JsonConvert.SerializeObject(applicationRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await HttpClient.PostAsync("https://localhost:5001/deployment", content);
            var message = await responseMessage.Content.ReadAsStringAsync();

            var error = JsonConvert.DeserializeObject<ErrorResponse>(message);
            
            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
            
        }
    }
}