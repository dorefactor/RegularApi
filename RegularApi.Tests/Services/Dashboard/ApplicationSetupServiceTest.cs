using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Dao.Model;
using RegularApi.Services.Dashboard;

namespace RegularApi.Tests.Services.Dashboard
{
    public class ApplicationSetupServiceTest
    {
        private Mock<IApplicationDao> _applicationDao;
        private ApplicationSetupService _applicationSetupService;

        [SetUp]
        public void SetUp()
        {
            _applicationDao = new Mock<IApplicationDao>();

            _applicationSetupService = new ApplicationSetupService(new LoggerFactory(), _applicationDao.Object);
        }

        [Test]
        public async Task TestSaveApplicationSetupAsync()
        {
            var expectedApplication = new Application() {
                Id = ObjectId.GenerateNewId(),
                Name = "a"
            };
            _applicationDao.Setup(dao => dao.SaveApplicationSetup(expectedApplication))
                .ReturnsAsync(Option<Application>.Some(expectedApplication));

            var actualApplicationStored = await _applicationSetupService.SaveApplicationSetupAsync(expectedApplication);

            Assert.False(actualApplicationStored.IsLeft);
            Assert.True(actualApplicationStored.IsRight);
            // Assert.AreEqual(actualApplicationStored, expectedApplication);

            _applicationDao.Verify(dao => dao.SaveApplicationSetup(expectedApplication));
            _applicationDao.VerifyNoOtherCalls();
        }

        // [Test]
        // public async Task TestSuccessApplicationDeploymentQueued()
        // {
        //     var application = BuildApplication();
        //     _applicationDao.Setup(dao => dao.GetApplicationByNameAsync(AppName))
        //         .ReturnsAsync(Option<Application>.Some(application));

        //     _rabbitMqTemplate.Setup(template => template.SendMessage(It.IsAny<string>()));

        //     var result = await _applicationSetupService.QueueDeploymentRequestAsync(AppName, Tag);

        //     Assert.True(result.IsRight);

        //     _applicationDao.Verify(dao => dao.GetApplicationByNameAsync(AppName));
        //     _rabbitMqTemplate.Verify(template => template.SendMessage(It.IsAny<string>()));

        //     _applicationDao.VerifyNoOtherCalls();
        //     _rabbitMqTemplate.VerifyNoOtherCalls();
        // }

        // [Test]
        // public async Task TestWhenExceptionReturnError()
        // {
        //     var application = BuildApplication();
        //     _applicationDao.Setup(dao => dao.GetApplicationByNameAsync(AppName))
        //         .ReturnsAsync(Option<Application>.Some(application));

        //     _rabbitMqTemplate.Setup(template => template.SendMessage(It.IsAny<string>()))
        //         .Throws(new Exception("expected exception"));

        //     var result = await _applicationSetupService.QueueDeploymentRequestAsync(AppName, Tag);

        //     var expectedError = "Can't queue deployment request for app: " + AppName;
        //     var error = result.Match(right => "", left => left);

        //     Assert.True(result.IsLeft);
        //     Assert.AreEqual(expectedError, error);

        //     _applicationDao.Verify(dao => dao.GetApplicationByNameAsync(AppName));
        //     _rabbitMqTemplate.Verify(template => template.SendMessage(It.IsAny<string>()));

        //     _applicationDao.VerifyNoOtherCalls();
        //     _rabbitMqTemplate.VerifyNoOtherCalls();
        // }

        // // ----------------------------------------------------------------------------------------------------

        // private static Application BuildApplication()
        // {
        //     return new Application
        //     {
        //         Id = ObjectId.GenerateNewId(),
        //         Name = AppName
        //     };
        // }
    }
}