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
        private Mock<ITransformer<ApplicationSetupView, ApplicationSetup>> _applicationSetupTransformer;
        private Mock<ApplicationSetup> _applicationSetup;
        private Mock<ApplicationSetupView> _applicationSetupView;

        private ITransformer<DeploymentOrderView, DeploymentOrder> _deploymentOrderTransformer;

        [SetUp]
        public void SetUp()
        {
            _applicationSetupTransformer = new Mock<ITransformer<ApplicationSetupView, ApplicationSetup>>();
            _applicationSetup = new Mock<ApplicationSetup>();
            _applicationSetupView = new Mock<ApplicationSetupView>();

            _deploymentOrderTransformer = new DeploymentOrderTransformer(_applicationSetupTransformer.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _applicationSetupTransformer.VerifyNoOtherCalls();
        }

        [Theory]
        public void TestTransformFromView(ApplicationType applicationType)
        {
            var deploymentOrderView = ViewFixture.BuildDeploymentOrderView(applicationType.ToString());

            _applicationSetup.Setup(_ => _.ApplicationType).Returns(applicationType);
            _applicationSetupTransformer.Setup(_ => _.Transform(deploymentOrderView.ApplicationSetupView))
                            .Returns(_applicationSetup.Object);

            var actualDeploymentOrder = _deploymentOrderTransformer.Transform(deploymentOrderView);

            _applicationSetupTransformer.Verify(_ => _.Transform(deploymentOrderView.ApplicationSetupView));

            actualDeploymentOrder.RequestId.Should().MatchRegex("\\b");
            actualDeploymentOrder.ApplicationSetup.ApplicationType.ToString().Should().BeEquivalentTo(deploymentOrderView.ApplicationSetupView.Type);
        }

        [Theory]
        public void TestTransformToView(ApplicationType applicationType)
        {
            var deploymentOrder = ModelFixture.BuildDeploymentOrder(applicationType);

            _applicationSetupView.Setup(_ => _.Type).Returns(applicationType.ToString());
            _applicationSetupTransformer.Setup(_ => _.Transform(deploymentOrder.ApplicationSetup))
                            .Returns(_applicationSetupView.Object);

            var actualDeploymentOrder = _deploymentOrderTransformer.Transform(deploymentOrder);

            _applicationSetupTransformer.Verify(_ => _.Transform(deploymentOrder.ApplicationSetup));

            actualDeploymentOrder.Should().NotBeNull();
            actualDeploymentOrder.RequestId.Should().MatchRegex("\\b");
            actualDeploymentOrder.ApplicationSetupView.Type.Should().BeEquivalentTo(deploymentOrder.ApplicationSetup.ApplicationType.ToString());
        }
    }
}