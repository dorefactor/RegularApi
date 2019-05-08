using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;
using RegularApi.Enums;
using RegularApi.Tests.Fixtures;
using RegularApi.Transformers;

namespace RegularApi.Tests.Controllers.Configuration
{
    public class DeploymentTemplatesControllerIT : BaseControllerIT
    {
        private const string DeploymentTemplatesUri = "/configuration/deployment-templates";

        private DaoFixture _daoFixture;
        private ITransformer<DeploymentTemplateView, DeploymentTemplate> _deploymentTemplateTransformer;

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();

            _daoFixture = ServiceProvider.GetRequiredService<DaoFixture>();
            _deploymentTemplateTransformer = ServiceProvider.GetRequiredService<ITransformer<DeploymentTemplateView, DeploymentTemplate>>();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public async Task TestNewAsync_Ok()
        {
            var deploymentTemplateView = GetPayloadViewFromJsonFile<DeploymentTemplateView>("../../../Samples/Controllers/Payloads/deployment-template.json");
            var responseMessage = await PerformPostAsync(deploymentTemplateView, DeploymentTemplatesUri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            var expectedView = await GetResponseView<NewResourceResponseView>(responseMessage);

            expectedView.Link.Should().Be(DeploymentTemplatesUri + "/" + deploymentTemplateView.Name);
        }

        [Test]
        public async Task TestNewAsyncWithInvalidData_ReturnError()
        {
            var deploymentTemplateView = new DeploymentTemplateView();

            var responseMessage = await PerformPostAsync(deploymentTemplateView, DeploymentTemplatesUri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task TestGetAsync_Success()
        {
            var templateName = "super-template";
            var deploymentTemplate = await _daoFixture.CreateDeploymentTemplateAsync(templateName, ApplicationType.Docker);

            var uri = DeploymentTemplatesUri + "/" + templateName;
            var responseMessage = await PerformGetAsync(uri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            var expectedView = _deploymentTemplateTransformer.Transform(deploymentTemplate);
            var actualView = await GetResponseView<DeploymentTemplateView>(responseMessage);

            actualView.Should().BeEquivalentTo(expectedView);
        }

        [Test]
        public async Task TestGetAsync_NonExistingDeploymentTemplate_ReturnError()
        {
            var uri = DeploymentTemplatesUri + "/non-existing";
            var responseMessage = await PerformGetAsync(uri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}