using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RegularApi.Domain.Views;
using RegularApi.Tests.Fixtures;
using RegularApi.Transformers;

namespace RegularApi.Tests.Controllers.Configuration
{
    public class DeploymentTemplatesControllerIT : BaseControllerIT
    {
        private const string TemplatesUri = "/configuration/deploymentTemplates";
        private DaoFixture _daoFixture;

        private IDeploymentTemplateTransformer _transformer;

        [SetUp]        
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();

            _daoFixture = ServiceProvider.GetRequiredService<DaoFixture>();
            _transformer = ServiceProvider.GetRequiredService<IDeploymentTemplateTransformer>();
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
        public async Task CreateDeploymentTemplateWithInvalidDataReturnErrorTest()
        {
            var deploymentTemplateView = new DeploymentTemplateView();

            var response = await PerformPostAsync(deploymentTemplateView, TemplatesUri);
            
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetSuccessDeploymentTemplateTest()
        {
            var templateName = "super-template";
            var deploymentTemplate = await _daoFixture.CreateDeploymentTemplateAsync(templateName);
            var expectedView = _transformer.ToResource(deploymentTemplate);

            var uri = TemplatesUri + "/" + templateName;
            var response = await PerformGetAsync(uri);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var view = await GetResponse<DeploymentTemplateView>(response);

            view.Should().BeEquivalentTo(expectedView);
        }

        [Test]
        public async Task GetNonExistingDeploymentTemplateReturnErrorTest()
        {
            var uri = TemplatesUri + "/non-existing";
            var response = await PerformGetAsync(uri);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}