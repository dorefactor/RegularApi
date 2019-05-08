using System;
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
    public class ApplicationTransformerTest
    {
        private Mock<ITransformer<ApplicationSetupView, ApplicationSetup>> _applicationSetupTransformer;
        private Mock<ApplicationSetup> _applicationSetup;

        private ITransformer<ApplicationView, Application> _applicationTransformer;

        [SetUp]
        public void SetUp()
        {
            _applicationSetupTransformer = new Mock<ITransformer<ApplicationSetupView, ApplicationSetup>>();
            _applicationSetup = new Mock<ApplicationSetup>();

            _applicationTransformer = new ApplicationTransformer(_applicationSetupTransformer.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _applicationSetupTransformer.VerifyNoOtherCalls();
        }

        [Theory]
        public void TestTransformFromView(ApplicationType applicationType)
        {
            var applicationView = ViewFixture.BuildApplicationView("application-test", applicationType.ToString());

            _applicationSetup.Setup(_ => _.ApplicationType).Returns(applicationType);
            _applicationSetupTransformer.Setup(_ => _.Transform(applicationView.ApplicationSetupView))
                .Returns(_applicationSetup.Object);

            var actualApplication = _applicationTransformer.Transform(applicationView);

            _applicationSetupTransformer.Verify(_ => _.Transform(applicationView.ApplicationSetupView));

            actualApplication.Name.Should().BeEquivalentTo(applicationView.Name);
            actualApplication.ApplicationSetup.ApplicationType.ToString().Should().BeEquivalentTo(applicationView.ApplicationSetupView.Type);
        }

        [Test]
        public void TestTransformToView_ReturnNotImplementedException()
        {
            var application = new Application();
            Assert.Throws<NotImplementedException>(() => _applicationTransformer.Transform(application));
        }
    }
}
