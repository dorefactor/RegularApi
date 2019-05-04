using System.Threading.Tasks;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.Services;

namespace RegularApi.Tests.Services
{
    public class ApplicationSetupServiceTest
    {
        private Mock<IApplicationDao> _applicationDao;
        private ApplicationService _applicationSetupService;

        [SetUp]
        public void SetUp()
        {
            _applicationDao = new Mock<IApplicationDao>();

            _applicationSetupService = new ApplicationService(new LoggerFactory(), _applicationDao.Object);
        }

        [Test]
        public async Task TestSaveApplicationSetupAsync()
        {
            var expectedApplication = new Application();
            _applicationDao.Setup(dao => dao.SaveAsync(expectedApplication))
                .ReturnsAsync(Option<Application>.Some(expectedApplication));

            var actualApplicationStored = await _applicationSetupService.AddApplicationSetupAsync(expectedApplication);

            Assert.False(actualApplicationStored.IsLeft);
            Assert.True(actualApplicationStored.IsRight);
            Assert.AreEqual(actualApplicationStored, expectedApplication);

            _applicationDao.Verify(dao => dao.SaveAsync(expectedApplication));
            _applicationDao.VerifyNoOtherCalls();
        }
    }
}