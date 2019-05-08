using System;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.RabbitMq.Templates;
using RegularApi.Services;

namespace RegularApi.Tests.Services
{
    public class DeploymentServiceTest
    {
        private Mock<ILogger<DeploymentService>> _logger;
        private Mock<IDeploymentTemplateDao> _deploymentTemplateDao;
        private Mock<IDeploymentOrderDao> _deploymentOrderDao;
        private Mock<IRabbitMqTemplate> _rabbitMqTemplate;

        private DeploymentService _deploymentService;

        [SetUp]
        public void SetUp()
        {
            _logger = new Mock<ILogger<DeploymentService>>();
            _deploymentTemplateDao = new Mock<IDeploymentTemplateDao>();
            _deploymentOrderDao = new Mock<IDeploymentOrderDao>();
            _rabbitMqTemplate = new Mock<IRabbitMqTemplate>();

            _deploymentService = new DeploymentService(
                _logger.Object,
                _deploymentTemplateDao.Object,
                _deploymentOrderDao.Object,
                _rabbitMqTemplate.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _logger.VerifyNoOtherCalls();
            _deploymentTemplateDao.VerifyNoOtherCalls();
            _deploymentTemplateDao.VerifyNoOtherCalls();
            _rabbitMqTemplate.VerifyNoOtherCalls();
        }

        [Test]
        public async Task TestQueueDeploymentOrderAsync_NoDeploymentTemplateExists_ReturnsError()
        {
            var deploymentTemplateId = new ObjectId();

            var deploymentOrder = new DeploymentOrder { DeploymentTemplateId = deploymentTemplateId };

            _deploymentTemplateDao.Setup(_ => _.GetByIdAsync(deploymentOrder.DeploymentTemplateId))
                 .ReturnsAsync(Option<DeploymentTemplate>.None);

            var deploymentOrderHolder = await _deploymentService.QueueDeploymentOrderAsync(deploymentOrder);

            var expectedError = "No deployment template_id found: " + deploymentTemplateId;

            deploymentOrderHolder.IsLeft.Should().BeTrue();
            deploymentOrderHolder.LeftAsEnumerable().First().Should().Be(expectedError);

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _deploymentTemplateDao.Verify(_ => _.GetByIdAsync(deploymentOrder.DeploymentTemplateId));
        }

        [Test]
        public async Task TestQueueDeploymentOrderAsync_Success()
        {
            var deploymentTemplateId = new ObjectId();
            var deploymentTemplate = new DeploymentTemplate { Id = deploymentTemplateId };
            var deploymentOrder = new DeploymentOrder { DeploymentTemplateId = deploymentTemplateId };

            _deploymentTemplateDao.Setup(_ => _.GetByIdAsync(deploymentOrder.DeploymentTemplateId))
                .ReturnsAsync(Option<DeploymentTemplate>.Some(deploymentTemplate));

            _rabbitMqTemplate.Setup(_ => _.SendMessage(It.IsAny<string>()));

            var actualDeploymentOrderQueued = await _deploymentService.QueueDeploymentOrderAsync(deploymentOrder);

            actualDeploymentOrderQueued.IsRight.Should().BeTrue();

            actualDeploymentOrderQueued.RightAsEnumerable().First().Should().BeEquivalentTo(deploymentOrder);

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.AtLeast(2));
            _deploymentTemplateDao.Verify(_ => _.GetByIdAsync(deploymentOrder.DeploymentTemplateId));
            _rabbitMqTemplate.Verify(_ => _.SendMessage(It.IsAny<string>()));
        }

        [Test]
        public async Task TestQueueDeploymentOrderAsync_ThrowsException_ReturnError()
        {
            var deploymentTemplateId = new ObjectId();
            var deploymentTemplate = new DeploymentTemplate { Id = deploymentTemplateId };
            var deploymentOrder = new DeploymentOrder { DeploymentTemplateId = deploymentTemplateId };

            _deploymentTemplateDao.Setup(_ => _.GetByIdAsync(deploymentOrder.DeploymentTemplateId))
                .ReturnsAsync(Option<DeploymentTemplate>.Some(deploymentTemplate));

            _rabbitMqTemplate.Setup(_ => _.SendMessage(It.IsAny<string>()))
                .Throws(new Exception("expected exception")); ;

            var actualDeploymentOrderQueued = await _deploymentService.QueueDeploymentOrderAsync(deploymentOrder);
            var expectedError = "Can't queue deployment request for template_id: " + deploymentTemplateId;

            actualDeploymentOrderQueued.IsLeft.Should().BeTrue();
            actualDeploymentOrderQueued.LeftAsEnumerable().First().Should().Be(expectedError);

            _logger.Verify(_ => _.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()), Times.AtLeast(2));
            _logger.Verify(_ => _.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<FormattedLogValues>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()));
            _deploymentTemplateDao.Verify(_ => _.GetByIdAsync(deploymentOrder.DeploymentTemplateId));
            _rabbitMqTemplate.Verify(_ => _.SendMessage(It.IsAny<string>()));
        }
    }
}