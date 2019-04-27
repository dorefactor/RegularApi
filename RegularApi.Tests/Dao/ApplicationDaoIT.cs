using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Domain.Model;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Dao
{
    public class ApplicationDaoIT : BaseDaoIT
    {
        private IApplicationDao _applicationDao;

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();
            DropCollection("applications");

            _applicationDao = GetDao<IApplicationDao>();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public async Task TestGetApplications()
        {
            var application = await GetDaoFixture().CreateApplication("super-application-2k");

            var apps = await _applicationDao.GetApplicationsAsync();

            Assert.NotNull(apps);

            var expected = apps.First(app => application.Id.Equals(app.Id));

            Assert.NotNull(expected);
        }

        [Test]
        public async Task TestGetNonExistingApplication()
        {
            var appHolder = await _applicationDao.GetApplicationByNameAsync("non-existing-app");

            Assert.NotNull(appHolder);
            Assert.True(appHolder.IsNone);
        }

        [Test]
        public async Task TestGetApplicationByName()
        {
            const string appName = "aka-aka-app";

            var application = await GetDaoFixture().CreateApplication(appName);

            var appHolder = await _applicationDao.GetApplicationByNameAsync(appName);

            Assert.NotNull(appHolder);

            var result = appHolder.AsEnumerable().First();

            Assert.AreEqual(application.Id, result.Id);
        }

        [Test]
        public async Task TestSaveApplicationSetup()
        {
            var expectedApplication = ModelFactory.CreateApplication();

            var applicationSetupHolder = await _applicationDao.SaveApplicationSetup(expectedApplication);

            Assert.NotNull(applicationSetupHolder);

            var actualApplication = GetDaoFixture().GetApplicationById(applicationSetupHolder.AsEnumerable().First().Id).Result;

            Assert.NotNull(actualApplication.Id);

            actualApplication.Should().BeEquivalentTo(expectedApplication);
        }
    }
}