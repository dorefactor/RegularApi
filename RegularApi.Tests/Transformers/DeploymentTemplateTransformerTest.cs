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
        private Mock<ITransformer<ApplicationView, Application>> _applicationTransformer;
        private Mock<Application> _application;
        private Mock<ApplicationView> _applicationView;

        private ITransformer<DeploymentTemplateView, DeploymentTemplate> _deploymentTemplateTransformer;

        [SetUp]
        public void SetUp()
        {
            _applicationTransformer = new Mock<ITransformer<ApplicationView, Application>>();
            _application = new Mock<Application>();
            _applicationView = new Mock<ApplicationView>();

            _deploymentTemplateTransformer = new DeploymentTemplateTransformer(_applicationTransformer.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _applicationTransformer.VerifyNoOtherCalls();
        }

        [Theory]
        public void TestTransformFromView(ApplicationType applicationType)
        {
            var deploymentTemplateView = ViewFixture.BuildDeploymentTemplateView("template-test", applicationType.ToString());

            _applicationTransformer.Setup(_ => _.Transform(deploymentTemplateView.ApplicationView))
                            .Returns(_application.Object);

            var actualDeploymentTemplate = _deploymentTemplateTransformer.Transform(deploymentTemplateView);

            _applicationTransformer.Verify(_ => _.Transform(deploymentTemplateView.ApplicationView));

            actualDeploymentTemplate.Name.Should().BeEquivalentTo(deploymentTemplateView.Name);
        }

        [Theory]
        public void TestTransformToView(ApplicationType applicationType)
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate("super-template", applicationType);

            _applicationTransformer.Setup(_ => _.Transform(deploymentTemplate.Application))
                            .Returns(_applicationView.Object);

            var actualDeploymentTemplateView = _deploymentTemplateTransformer.Transform(deploymentTemplate);

            _applicationTransformer.Verify(_ => _.Transform(deploymentTemplate.Application));

            actualDeploymentTemplateView.Should().NotBeNull();
            actualDeploymentTemplateView.Name.Should().Be("super-template");
        }
    }
}