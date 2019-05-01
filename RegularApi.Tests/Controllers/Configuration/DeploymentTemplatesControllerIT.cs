using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RegularApi.Domain.Views;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Controllers.Configuration
{
    public class DeploymentTemplatesControllerIT : BaseControllerIT
    {
        private const string TemplatesUri = "/configuration/deploymentTemplates";

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
        public async Task CreateDeploymentTemplateTest()
        {
            var templateName = "super-template";
            var deploymentTemplateView = ViewFactory.BuildDeploymentTemplateView(templateName);

            var response = await PerformPostAsync(deploymentTemplateView, TemplatesUri);
            
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var expectedResponse = await GetResponse<NewResourceResponseView>(response);

            expectedResponse.Link.Should().Be("/configuration/deploymentTemplates/" + templateName);
        }

        [Test]
        public async Task CreateDeploymentTemplateErrorTest()
        {
            var deploymentTemplateView = new DeploymentTemplateView();

            var response = await PerformPostAsync(deploymentTemplateView, TemplatesUri);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var cosa = await response.Content.ReadAsStringAsync();
        }
    }
}