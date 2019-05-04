using FluentAssertions;
using NUnit.Framework;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Transformers
{
    public class DeploymentTemplateTransformerTest
    {
        private IDeploymentTemplateTransformer _transformer;

        [SetUp]
        public void SetUp()
        {
            _transformer = new DeploymentTemplateTransformer();
        }

        [Test]
        public void TestTransformFromResource()
        {
            var deploymentTemplateView = ViewFixture.BuildDeploymentTemplateView("super-template");

            var deploymentTemplate = _transformer.FromView(deploymentTemplateView);

            deploymentTemplate.Should().NotBeNull();
            deploymentTemplate.Name.Should().Be("super-template");
        }

        [Test]
        public void TestTransformToResource()
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate("super-template");

            var deploymentTemplateView = _transformer.ToView(deploymentTemplate);

            deploymentTemplateView.Should().NotBeNull();
            deploymentTemplateView.Name.Should().Be("super-template");
        }
    }
}