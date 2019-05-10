using FluentAssertions;
using Moq;
using NUnit.Framework;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;
using RegularApi.Enums;
using RegularApi.Tests.Fixtures;
using RegularApi.Transformers;

namespace RegularApi.Tests.Transformers
{
    public class DeploymentOrderTransformerTest
    {
        private Mock<ITransformer<ApplicationView, Application>> _applicationTransformer;
        private Mock<Application> _application;
        private Mock<ApplicationView> _applicationView;
        private ITransformer<DeploymentOrderView, DeploymentOrder> _deploymentOrderTransformer;

        [SetUp]
        public void SetUp()
        {
            _applicationTransformer = new Mock<ITransformer<ApplicationView, Application>>();
            _application = new Mock<Application>();
            _applicationView = new Mock<ApplicationView>();

            _deploymentOrderTransformer = new DeploymentOrderTransformer(_applicationTransformer.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _applicationTransformer.VerifyNoOtherCalls();
        }

        [Theory]
        public void TestTransformFromView(ApplicationType applicationType)
        {
            var deploymentOrderView = ViewFixture.BuildDeploymentOrderView(applicationType.ToString());

            _applicationTransformer.Setup(_ => _.Transform(deploymentOrderView.ApplicationView))
                            .Returns(_application.Object);

            var actualDeploymentOrder = _deploymentOrderTransformer.Transform(deploymentOrderView);

            _applicationTransformer.Verify(_ => _.Transform(deploymentOrderView.ApplicationView));

            actualDeploymentOrder.RequestId.Should().MatchRegex("\\b");
        }

        [Theory]
        public void TestTransformToView(ApplicationType applicationType)
        {
            var deploymentOrder = ModelFixture.BuildDeploymentOrder(applicationType);

            _applicationTransformer.Setup(_ => _.Transform(deploymentOrder.Application))
                            .Returns(_applicationView.Object);

            var actualDeploymentOrder = _deploymentOrderTransformer.Transform(deploymentOrder);

            _applicationTransformer.Verify(_ => _.Transform(deploymentOrder.Application));

            actualDeploymentOrder.Should().NotBeNull();
            actualDeploymentOrder.RequestId.Should().MatchRegex("\\b");
        }
    }
}