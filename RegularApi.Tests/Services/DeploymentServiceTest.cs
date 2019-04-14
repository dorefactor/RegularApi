using System;
using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Dao.Model;
using RegularApi.RabbitMq.Templates;
using RegularApi.Services;
using RegularApi.Services.Domain;

namespace RegularApi.Tests.Services
{
    public class DeploymentServiceTest
    {
        private const string AppName = "super-app";
        private const string Tag = "1.1.1";

        private Mock<IApplicationDao> _applicationDao;
        private Mock<IRabbitMqTemplate> _rabbitMqTemplate;
        private DeploymentService _deploymentService;

        [SetUp]
        public void SetUp()
        {
            _applicationDao = new Mock<IApplicationDao>();
            _rabbitMqTemplate = new Mock<IRabbitMqTemplate>();

            _deploymentService = new DeploymentService(new LoggerFactory(), _applicationDao.Object, _rabbitMqTemplate.Object);
        }

        [Test]
        public async Task TestWhenNoApplicationExistsReturnsError()
        {
            _applicationDao.Setup(dao => dao.GetApplicationByNameAsync(AppName))
                .ReturnsAsync(Option<Application>.None);

            var result = await _deploymentService.QueueDeploymentRequestAsync(AppName, Tag);

            var expectedError = "No application found with name: " + AppName;
            var error = result.Match(right => "", left => left);

            Assert.True(result.IsLeft);
            Assert.AreEqual(expectedError, error);

            _applicationDao.Verify(dao => dao.GetApplicationByNameAsync(AppName));
            _applicationDao.VerifyNoOtherCalls();
            _rabbitMqTemplate.VerifyNoOtherCalls();
        }

        [Test]
        public async Task TestSuccessApplicationDeploymentQueued()
        {
            var application = BuildApplication();
            _applicationDao.Setup(dao => dao.GetApplicationByNameAsync(AppName))
                .ReturnsAsync(Option<Application>.Some(application));

            _rabbitMqTemplate.Setup(template => template.SendMessage(It.IsAny<string>()));

            var result = await _deploymentService.QueueDeploymentRequestAsync(AppName, Tag);

            Assert.True(result.IsRight);

            _applicationDao.Verify(dao => dao.GetApplicationByNameAsync(AppName));
            _rabbitMqTemplate.Verify(template => template.SendMessage(It.IsAny<string>()));

            _applicationDao.VerifyNoOtherCalls();
            _rabbitMqTemplate.VerifyNoOtherCalls();
        }

        [Test]
        public async Task TestWhenExceptionReturnError()
        {
            var application = BuildApplication();
            _applicationDao.Setup(dao => dao.GetApplicationByNameAsync(AppName))
                .ReturnsAsync(Option<Application>.Some(application));

            _rabbitMqTemplate.Setup(template => template.SendMessage(It.IsAny<string>()))
                .Throws(new Exception("expected exception"));

            var result = await _deploymentService.QueueDeploymentRequestAsync(AppName, Tag);

            var expectedError = "Can't queue deployment request for app: " + AppName;
            var error = result.Match(right => "", left => left);

            Assert.True(result.IsLeft);
            Assert.AreEqual(expectedError, error);

            _applicationDao.Verify(dao => dao.GetApplicationByNameAsync(AppName));
            _rabbitMqTemplate.Verify(template => template.SendMessage(It.IsAny<string>()));

            _applicationDao.VerifyNoOtherCalls();
            _rabbitMqTemplate.VerifyNoOtherCalls();
        }

        // ----------------------------------------------------------------------------------------------------

        private static Application BuildApplication()
        {
            return new Application
            {
                Id = ObjectId.GenerateNewId(),
                Name = AppName
            };
        }
    }
}