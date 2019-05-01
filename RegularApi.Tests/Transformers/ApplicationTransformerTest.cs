using FluentAssertions;
using NUnit.Framework;
using RegularApi.Tests.Fixtures;
using RegularApi.Transformers;

namespace RegularApi.Tests.Transformers
{
    public class ApplicationTransformerTest
    {
        private IApplicationTransformer _applicationTransformer;

        [SetUp]
        public void SetUp()
        {
            _applicationTransformer = new ApplicationTransformer();
        }

        [Test]
        public void TestTransformFromResource()
        {
            var applicationResource = ViewFactory.BuildApplicationView();
            var application = _applicationTransformer.FromResource(applicationResource);

            application.Name.Should().BeSameAs(applicationResource.Name);
            application.DockerSetup.Should().BeEquivalentTo(applicationResource.DockerSetup);
        }
    }
}
