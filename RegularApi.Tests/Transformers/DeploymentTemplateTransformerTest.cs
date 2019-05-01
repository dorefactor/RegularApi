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
        public void TestDeploymentTemplateTransform()
        {
            var deploymentTemplateView = ViewFactory.BuildDeploymentTemplateView("super-template");

            var deploymentTemplate = _transformer.FromResource(deploymentTemplateView);

            deploymentTemplate.Should().NotBeNull();
            deploymentTemplate.Name.Should().Be("super-template");
        }
    }
}