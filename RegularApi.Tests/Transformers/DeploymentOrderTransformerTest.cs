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
    public class DeploymentTemplateTransformerTest
    {
        private Mock<ITransformer<ApplicationSetupView, ApplicationSetup>> _applicationSetupTransformer;
        private Mock<ApplicationSetup> _applicationSetup;
        private Mock<ApplicationSetupView> _applicationSetupView;

        private ITransformer<DeploymentTemplateView, DeploymentTemplate> _deploymentTemplateTransformer;

        [SetUp]
        public void SetUp()
        {
            _applicationSetupTransformer = new Mock<ITransformer<ApplicationSetupView, ApplicationSetup>>();
            _applicationSetup = new Mock<ApplicationSetup>();
            _applicationSetupView = new Mock<ApplicationSetupView>();

            _deploymentTemplateTransformer = new DeploymentTemplateTransformer(_applicationSetupTransformer.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _applicationSetupTransformer.VerifyNoOtherCalls();
        }

        [Theory]
        public void TestTransformFromView(ApplicationType applicationType)
        {
            var deploymentTemplateView = ViewFixture.BuildDeploymentTemplateView("template-test", applicationType.ToString());

            _applicationSetup.Setup(_ => _.ApplicationType).Returns(applicationType);
            _applicationSetupTransformer.Setup(_ => _.Transform(deploymentTemplateView.ApplicationSetupView))
                            .Returns(_applicationSetup.Object);

            var actualDeploymentTemplate = _deploymentTemplateTransformer.Transform(deploymentTemplateView);

            _applicationSetupTransformer.Verify(_ => _.Transform(deploymentTemplateView.ApplicationSetupView));

            actualDeploymentTemplate.Name.Should().BeEquivalentTo(deploymentTemplateView.Name);
            actualDeploymentTemplate.ApplicationSetup.ApplicationType.ToString().Should().BeEquivalentTo(deploymentTemplateView.ApplicationSetupView.Type);
        }

        [Theory]
        public void TestTransformToView(ApplicationType applicationType)
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate("super-template", applicationType);

            _applicationSetupView.Setup(_ => _.Type).Returns(applicationType.ToString());
            _applicationSetupTransformer.Setup(_ => _.Transform(deploymentTemplate.ApplicationSetup))
                            .Returns(_applicationSetupView.Object);

            var actualDeploymentTemplate = _deploymentTemplateTransformer.Transform(deploymentTemplate);

            _applicationSetupTransformer.Verify(_ => _.Transform(deploymentTemplate.ApplicationSetup));

            actualDeploymentTemplate.Should().NotBeNull();
            actualDeploymentTemplate.Name.Should().Be("super-template");
            actualDeploymentTemplate.ApplicationSetupView.Type.Should().BeEquivalentTo(deploymentTemplate.ApplicationSetup.ApplicationType.ToString());
        }
    }
}