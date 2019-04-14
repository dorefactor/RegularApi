using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using NUnit.Framework;
using RegularApi.Controllers.Deployment.Views;
using RegularApi.Controllers.Views;
using RegularApi.Dao.Model;

namespace RegularApi.Tests.Controllers.Deployment
{
    public class DeploymentControllerTest : IntegrationTestBase
    {
        private const string DeploymentUri = "https://localhost:5001/deployment";

        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public async Task TestValidationErrors()
        {
            var applicationRequest = new ApplicationRequest();

            var responseMessage = await PerformPostAsync(applicationRequest, DeploymentUri);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);            
        }

        [Test]
        public async Task TestDeployNotExistingAppReturnError()
        {
            var applicationRequest = CreateApplicationRequest();

            var responseMessage = await PerformPostAsync(applicationRequest, DeploymentUri);
            var response = await GetResponse<ErrorResponse>(responseMessage);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, responseMessage.StatusCode);
            Assert.AreEqual("No application found with name: " + applicationRequest.Name, response.Error);
        }

        [Test]
        public async Task TestDeploymentRequestIsSuccessQueue()
        {
            var applicationRequest = CreateApplicationRequest();

            var application = await CreateApplication(applicationRequest.Name);
            
            var responseMessage = await PerformPostAsync(applicationRequest, DeploymentUri);
            var response = await GetResponse<ApplicationResponse>(responseMessage);

            await DeleteApplication(application.Id);
            
            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

            Assert.NotNull(response.DeploymentId);
            Assert.NotNull(response.Received);
            Assert.AreEqual(applicationRequest.Name, response.Name);
            Assert.AreEqual(applicationRequest.Tag, response.Tag);
        }

        // ------------------------------------------------------------------------------------------------
        
        private async Task<HttpResponseMessage> PerformPostAsync<T>(T request, string uri)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var responseMessage = await HttpClient.PostAsync(uri, content);

            return responseMessage;
        }

        private async Task<T> GetResponse<T>(HttpResponseMessage responseMessage)
        {
            var content = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        private IMongoCollection<Application> GetApplicationCollection()
        {
            var client = (IMongoClient) ServiceProvider.GetService(typeof(IMongoClient));
            var configuration = (IConfiguration) ServiceProvider.GetService(typeof(IConfiguration));

            var databaseName = configuration["MongoDb:Database"];
            var dataBase = client.GetDatabase(databaseName);
            return dataBase.GetCollection<Application>("applications");            
        }
        private async Task<Application> CreateApplication(string name)
        {
            var collection = GetApplicationCollection();
            
            var application = new Application
            {
                Name = name
            };

            await collection.InsertOneAsync(application);

            return application;
        }

        private async Task DeleteApplication(ObjectId id)
        {
            var collection = GetApplicationCollection();

            var filter = new FilterDefinitionBuilder<Application>()
                .Where(app => app.Id.Equals(id));
            
            await collection.DeleteOneAsync(filter);
        }
                
        private static ApplicationRequest CreateApplicationRequest()
        {
            return new ApplicationRequest
            {
                Tag = "1.1.1",
                Name = "SuperApplication"
            };
        }               
    }
}