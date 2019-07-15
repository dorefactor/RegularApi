using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using LanguageExt;
using Moq;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.Services;

namespace RegularApi.Tests.Services
{
    public class ApplicationServiceTest
    {
        private Mock<IApplicationDao> _applicationDao;

        private ApplicationService _applicationSetupService;

        [SetUp]
        public void SetUp()
        {
            _applicationDao = new Mock<IApplicationDao>();

            _applicationSetupService = new ApplicationService(_applicationDao.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _applicationDao.VerifyNoOtherCalls();
        }

        [Test]
        public async Task TestAddApplicationSetupAsync()
        {
            var expectedApplication = new Application();

            _applicationDao.Setup(dao => dao.SaveAsync(expectedApplication))
                .ReturnsAsync(Option<Application>.Some(expectedApplication));

            var actualApplication = await _applicationSetupService.AddApplicationSetupAsync(expectedApplication);

            actualApplication.IsLeft.Should().BeFalse();
            actualApplication.IsRight.Should().BeTrue();

            actualApplication.RightAsEnumerable().FirstOrDefault().Should().BeEquivalentTo(expectedApplication);

            _applicationDao.Verify(dao => dao.SaveAsync(expectedApplication));
        }

        [Test]
        public async Task TestGetAllApplicationsAsync()
        {
            var expectedApplications = new List<Application>() { new Application() }; 

            _applicationDao.Setup(dao => dao.GetAllAsync())
                .ReturnsAsync(expectedApplications);

            var actualApplications = await _applicationSetupService.GetAllApplicationsAsync();

            actualApplications.IsLeft.Should().BeFalse();
            actualApplications.IsRight.Should().BeTrue();

            actualApplications.Should().NotBeEmpty();

            _applicationDao.Verify(dao => dao.GetAllAsync());
        }
    }
}