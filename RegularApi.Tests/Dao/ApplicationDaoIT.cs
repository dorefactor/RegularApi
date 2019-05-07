using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Dao
{
    public class ApplicationDaoIT : BaseDaoIT
    {
        private IApplicationDao _applicationDao;
        private DaoFixture _daoFixture;

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();
            DropCollection(ApplicationDao.ApplicationCollectionName);

            _applicationDao = GetDao<IApplicationDao>();
            _daoFixture = GetDaoFixture();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public async Task TestGetAllAsync()
        {
            var application = await _daoFixture.CreateApplicationAsync("super-application-2k");

            var applications = await _applicationDao.GetAllAsync();

            applications.Should().NotBeEmpty();

            var actual = applications.First(_ => application.Id.Equals(_.Id));

            actual.Should().NotBeNull();

            actual.Should().BeEquivalentTo(application);
        }

        [Test]
        public async Task TestGetNonExisting()
        {
            var applicationHolder = await _applicationDao.GetByNameAsync("non-existing-app");

            applicationHolder.IsNone.Should().BeTrue();
            applicationHolder.FirstOrDefault().Should().BeNull();
        }

        [Test]
        public async Task TestGetByNameAsync()
        {
            string applicationName = "aka-aka-app";

            var expectedApplication = await _daoFixture.CreateApplicationAsync(applicationName);

            var applicationHolder = await _applicationDao.GetByNameAsync(applicationName);

            applicationHolder.FirstOrDefault().Should().NotBeNull();

            var actualApplication = applicationHolder.FirstOrDefault();

            actualApplication.Id.Should().BeEquivalentTo(expectedApplication.Id);
        }

        [Test]
        public async Task TestSaveAsync()
        {
            var expectedApplication = ModelFixture.BuildApplication("test");

            var applicationSetupHolder = await _applicationDao.SaveAsync(expectedApplication);

            applicationSetupHolder.FirstOrDefault().Should().NotBeNull();

            var actualApplication = await _daoFixture.GetApplicationByIdAsync(applicationSetupHolder.FirstOrDefault().Id);

            actualApplication.Should().NotBeNull();
            actualApplication.Should().BeEquivalentTo(expectedApplication);
        }
    }
}