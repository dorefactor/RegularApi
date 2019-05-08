using FluentAssertions;
using NUnit.Framework;
using RegularApi.Domain.Model;
using RegularApi.Domain.Model.Docker;
using RegularApi.Domain.Views;
using RegularApi.Domain.Views.Docker;
using RegularApi.Enums;
using RegularApi.Tests.Fixtures;
using RegularApi.Transformers;

namespace RegularApi.Tests.Transformers
{
    public class ApplicationSetupTransformerTest
    {
        private ITransformer<ApplicationSetupView, ApplicationSetup> _applicationSetupTransformer;

        [SetUp]
        public void SetUp()
        {
            _applicationSetupTransformer = new ApplicationSetupTransformer();
        }

        [Test]
        public void TestTransformFromView()
        {
            var applicationSetupView = (DockerApplicationSetupView)ViewFixture.BuildDockerApplicationSetupView(ApplicationType.Docker.ToString());

            var actualApplicationSetup = (DockerApplicationSetup)_applicationSetupTransformer.Transform(applicationSetupView);

            actualApplicationSetup.ApplicationType.ToString().Should().BeEquivalentTo(applicationSetupView.Type);
            actualApplicationSetup.Registry.Should().BeEquivalentTo(applicationSetupView.RegistryView);
            actualApplicationSetup.Image.Should().BeEquivalentTo(applicationSetupView.ImageView);
            actualApplicationSetup.EnvironmentVariables.Should().BeEquivalentTo(applicationSetupView.EnvironmentVariables);
            actualApplicationSetup.Ports.Should().BeEquivalentTo(applicationSetupView.Ports);
        }

        [Test]
        public void TestTransformToView_ReturnNotImplementedException()
        {
            var application = ModelFixture.BuildApplication("test", ApplicationType.Docker);
            var applicationSetup = (DockerApplicationSetup)application.ApplicationSetup;

            var actualApplication = (DockerApplicationSetupView)_applicationSetupTransformer.Transform(application.ApplicationSetup);

            actualApplication.Type.Should().BeEquivalentTo(applicationSetup.ApplicationType.ToString());
            actualApplication.RegistryView.Should().BeEquivalentTo(applicationSetup.Registry);
            actualApplication.ImageView.Should().BeEquivalentTo(applicationSetup.Image);
            actualApplication.EnvironmentVariables.Should().BeEquivalentTo(applicationSetup.EnvironmentVariables);
            actualApplication.Ports.Should().BeEquivalentTo(applicationSetup.Ports);
        }
    }
}
