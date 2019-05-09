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
        private Mock<ApplicationSetupView> _applicationSetupView;

        private ITransformer<ApplicationView, Application> _applicationTransformer;

        [SetUp]
        public void SetUp()
        {
            _applicationSetupTransformer = new Mock<ITransformer<ApplicationSetupView, ApplicationSetup>>();
            _applicationSetup = new Mock<ApplicationSetup>();
            _applicationSetupView = new Mock<ApplicationSetupView>();

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

        [Theory]
        public void TestTransformToView(ApplicationType applicationType)
        {
            var application = ModelFixture.BuildApplication("application-test", applicationType);

            _applicationSetupView.Setup(_ => _.Type).Returns(applicationType.ToString());
            _applicationSetupTransformer.Setup(_ => _.Transform(application.ApplicationSetup))
                .Returns(_applicationSetupView.Object);

            var actualApplicationView = _applicationTransformer.Transform(application);

            _applicationSetupTransformer.Verify(_ => _.Transform(application.ApplicationSetup));

            actualApplicationView.Name.Should().BeEquivalentTo(application.Name);
            actualApplicationView.ApplicationSetupView.Type.Should().BeEquivalentTo(application.ApplicationSetup.ApplicationType.ToString());
        }
    }
}
