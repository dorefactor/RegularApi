using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RegularApi.Domain.Views;
using RegularApi.Services.Deployment.Views;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Controllers.Deployment
{
    public class DeploymentControllerIT : BaseControllerIT
    {
        private const string DEPLOYMENT_URI = "/deployment";

        private DaoFixture _daoFixture;

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();

            _daoFixture = ServiceProvider.GetRequiredService<DaoFixture>();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public async Task TestValidationErrors()
        {
            var applicationRequest = new ApplicationRequest();

            var responseMessage = await PerformPostAsync(applicationRequest, DEPLOYMENT_URI);

            Assert.AreEqual(HttpStatusCode.BadRequest, responseMessage.StatusCode);
        }

        [Test]
        public async Task TestDeployNotExistingAppReturnError()
        {
            var applicationRequest = CreateApplicationRequest("test");

            var responseMessage = await PerformPostAsync(applicationRequest, DEPLOYMENT_URI);
            var response = await GetResponse<ErrorResponseView>(responseMessage);

            Assert.AreEqual(HttpStatusCode.UnprocessableEntity, responseMessage.StatusCode);
            Assert.AreEqual("No application found with name: " + applicationRequest.Name, response.Error);
        }

        [Test]
        public async Task TestDeploymentRequestIsSuccessQueue()
        {
            string applicationName = "test";

            await _daoFixture.CreateApplication(applicationName);

            var applicationRequest = CreateApplicationRequest(applicationName);

            var responseMessage = await PerformPostAsync(applicationRequest, DEPLOYMENT_URI);
            var response = await GetResponse<ApplicationResponse>(responseMessage);

            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);

            Assert.NotNull(response.DeploymentId);
            Assert.NotNull(response.Received);
            Assert.AreEqual(applicationRequest.Name, response.Name);
            Assert.AreEqual(applicationRequest.Tag, response.Tag);
        }

        private ApplicationRequest CreateApplicationRequest(string name)
        {
            return new ApplicationRequest
            {
                Tag = "1.1.1",
                Name = name
            };
        }
    }
}