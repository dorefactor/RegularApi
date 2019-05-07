using System;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;
using RegularApi.Tests.Fixtures;
using RegularApi.Transformers;

namespace RegularApi.Tests.Controllers.Deployment
{
    public class DeploymentOrderControllerIT : BaseControllerIT
    {
        private const string DeploymentOrdersUri = "/deployment-orders";

        private DaoFixture _daoFixture;
        private ITransformer<DeploymentOrderView, DeploymentOrder> _deploymentOrderTransformer;

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();

            _daoFixture = ServiceProvider.GetRequiredService<DaoFixture>();
            _deploymentOrderTransformer = ServiceProvider.GetRequiredService<ITransformer<DeploymentOrderView, DeploymentOrder>>();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public async Task TestNewAsync_Ok()
        {
            var templateName = "todo-app";
            await _daoFixture.CreateDeploymentTemplateAsync(templateName);

            var deploymentOrderView = GetPayloadViewFromJsonFile<DeploymentOrderView>("../../../Samples/Controllers/Payloads/deployment-order.json");
            var responseMessage = await PerformPostAsync(deploymentOrderView, DeploymentOrdersUri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            var actualView = await GetResponseView<NewResourceResponseView>(responseMessage);

            actualView.Link.Should().MatchRegex(DeploymentOrdersUri + "/\\b");
        }

        [Test]
        public async Task TestNewAsyncWithInvalidData_ReturnError()
        {
            var deploymentOrderView = new DeploymentOrderView();

            var responseMessage = await PerformPostAsync(deploymentOrderView, DeploymentOrdersUri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task TestGetAsync_Success()
        {
            var requestId = Guid.NewGuid().ToString();
            var deploymentOrder = await _daoFixture.CreateDeploymentOrderAsync(requestId);

            var uri = DeploymentOrdersUri + "/" + requestId;
            var responseMessage = await PerformGetAsync(uri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            deploymentOrder = await _daoFixture.GetDeploymentOrderByIdAsync(deploymentOrder.Id.ToString());

            var expectedView = _deploymentOrderTransformer.Transform(deploymentOrder);
            var actualView = await GetResponseView<DeploymentOrderView>(responseMessage);

            actualView.RequestId.Should().BeEquivalentTo(expectedView.RequestId);
        }

        [Test]
        public async Task TestGetAsync_NonExistingDeploymentOrder_ReturnError()
        {
            var uri = DeploymentOrdersUri + "/non-existing";
            var response = await PerformGetAsync(uri);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}