using System;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Services;
using Moq;
using RegularApi.Domain.Model;
using RegularApi.Tests.Fixtures;
using RegularApi.Enums;
using Microsoft.Extensions.Logging.Internal;

namespace RegularApi.Tests.Services
{
    public class DeploymentTemplateServiceTest
    {
        private const string TemplateName = "template";

        private Mock<ILogger<DeploymentTemplateService>> _logger;
        private Mock<IDeploymentTemplateDao> _deploymentTemplateDao;

        private DeploymentTemplateService _deploymentTemplateService;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<DeploymentTemplateService>>();
            _deploymentTemplateDao = new Mock<IDeploymentTemplateDao>();

            _deploymentTemplateService = new DeploymentTemplateService(
                _logger.Object,
                _deploymentTemplateDao.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _logger.VerifyNoOtherCalls();
            _deploymentTemplateDao.VerifyNoOtherCalls();
        }

        [Theory]
        public async Task TestGetDeploymentTemplateByNameAsync(ApplicationType applicationType)
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate(TemplateName, applicationType);

            _deploymentTemplateDao.Setup(_ => _.GetByNameAsync(TemplateName))
                .ReturnsAsync(Option<DeploymentTemplate>.Some(deploymentTemplate));

            var actualDeploymentTemplateHolder = await _deploymentTemplateService.GetDeploymentTemplateByNameAsync(TemplateName);

            actualDeploymentTemplateHolder.IsRight.Should().BeTrue();

            var actualDeploymentTemplate = actualDeploymentTemplateHolder.RightAsEnumerable().First();

            actualDeploymentTemplate.Should().BeEquivalentTo(deploymentTemplate);

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.AtLeast(2));
            _deploymentTemplateDao.Verify(_ => _.GetByNameAsync(TemplateName));
        }

        [Test]
        public async Task TestGetDeploymentTemplateByNameAsync_ReturnNone()
        {
            _deploymentTemplateDao.Setup(_ => _.GetByNameAsync(TemplateName))
                .ReturnsAsync(Option<DeploymentTemplate>.None);

            var actualDeploymentTemplateHolder = await _deploymentTemplateService.GetDeploymentTemplateByNameAsync(TemplateName);

            actualDeploymentTemplateHolder.IsLeft.Should().BeTrue();

            var actualError = actualDeploymentTemplateHolder.LeftAsEnumerable().First();

            actualError.Should().Be("Deployment template: " + TemplateName + " not found");

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _logger.Verify(_ => _.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _deploymentTemplateDao.Verify(_ => _.GetByNameAsync(TemplateName));
        }

        [Test]
        public async Task TestGetDeploymentTemplateByNameAsync_ThrowsException()
        {
            _deploymentTemplateDao.Setup(_ => _.GetByNameAsync(TemplateName))
                 .Throws<Exception>();

            var actualDeploymentTemplateHolder = await _deploymentTemplateService.GetDeploymentTemplateByNameAsync(TemplateName);

            actualDeploymentTemplateHolder.IsLeft.Should().BeTrue();

            var actualError = actualDeploymentTemplateHolder.LeftAsEnumerable().First();

            actualError.Should().Be("Can't get deployment template: " + TemplateName);

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _logger.Verify(_ => _.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _deploymentTemplateDao.Verify(_ => _.GetByNameAsync(TemplateName));
        }

        [Theory]
        public async Task TestAddDeploymentTemplateAsync(ApplicationType applicationType)
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate(TemplateName, applicationType);

            _deploymentTemplateDao.Setup(_ => _.GetByNameAsync(TemplateName))
                .ReturnsAsync(Option<DeploymentTemplate>.None);

            _deploymentTemplateDao.Setup(dao => dao.SaveAsync(deploymentTemplate))
                .ReturnsAsync(deploymentTemplate);

            var actualDeploymentTemplateHolder = await _deploymentTemplateService.AddDeploymentTemplateAsync(deploymentTemplate);

            actualDeploymentTemplateHolder.IsRight.Should().BeTrue();

            var actualDeploymentTemplate = actualDeploymentTemplateHolder.RightAsEnumerable().First();

            actualDeploymentTemplate.Should().BeEquivalentTo(deploymentTemplate);

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.AtLeast(2));
            _deploymentTemplateDao.Verify(_ => _.GetByNameAsync(TemplateName));
            _deploymentTemplateDao.Verify(_ => _.SaveAsync(deploymentTemplate));
        }

        [Theory]
        public async Task TestAddExistingDeploymentTemplateAsync_ReturnsError(ApplicationType applicationType)
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate(TemplateName, applicationType);

            _deploymentTemplateDao.Setup(_ => _.GetByNameAsync(TemplateName))
                 .ReturnsAsync(Option<DeploymentTemplate>.Some(deploymentTemplate));

            var actualDeploymentTemplateHolder = await _deploymentTemplateService.AddDeploymentTemplateAsync(deploymentTemplate);

            actualDeploymentTemplateHolder.IsLeft.Should().BeTrue();

            var actualError = actualDeploymentTemplateHolder.LeftAsEnumerable().First();

            actualError.Should().BeEquivalentTo("Deployment template: " + TemplateName + " already exists");

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _logger.Verify(_ => _.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _deploymentTemplateDao.Verify(_ => _.GetByNameAsync(TemplateName));
        }

        [Theory]
        public async Task TestAddDeploymentTemplateAsync_ThrowsException(ApplicationType applicationType)
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate(TemplateName, applicationType);

            _deploymentTemplateDao.Setup(_ => _.GetByNameAsync(TemplateName))
                 .ReturnsAsync(Option<DeploymentTemplate>.None);

            _deploymentTemplateDao.Setup(_ => _.SaveAsync(deploymentTemplate))
                .Throws<Exception>();

            var actualDeploymentTemplateHolder = await _deploymentTemplateService.AddDeploymentTemplateAsync(deploymentTemplate);

            actualDeploymentTemplateHolder.IsLeft.Should().BeTrue();

            var actualError = actualDeploymentTemplateHolder.LeftAsEnumerable().First();

            actualError.Should().BeEquivalentTo("Can't create deployment template: " + TemplateName);

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _logger.Verify(_ => _.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _deploymentTemplateDao.Verify(_ => _.GetByNameAsync(TemplateName));
            _deploymentTemplateDao.Verify(_ => _.SaveAsync(deploymentTemplate));
        }
    }
}