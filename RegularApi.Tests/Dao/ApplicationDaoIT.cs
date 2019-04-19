using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Dao.Model;

namespace RegularApi.Tests.Dao
{
    public class ApplicationDaoIT : BaseDaoIT
    {
        private IApplicationDao _applicationDao;

        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
            DropCollection("applications");
            _applicationDao = GetDao<IApplicationDao>();
        }

        [TearDown]
        public void TearDown()
        {
            MongoDbRunner.Dispose();
        }

        [Test]
        public async Task TestGetApplications()
        {
            var application = await CreateApplication("super-application-2k");

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
            var appName = "aka-aka-app";

            var application = await CreateApplication(appName);

            var appHolder = await _applicationDao.GetApplicationByNameAsync(appName);

            Assert.NotNull(appHolder);

            var result = appHolder.AsEnumerable().First();

            Assert.AreEqual(application.Id, result.Id);
        }

        [Test]
        public async Task TestSaveApplicationSetup()
        {
            var expectedApplication = new Application()
            {
                Name = "test-app",
                DockerSetup = new DockerSetup()
                {
                    ImageName = "image-name",
                    RegistryUrl = "registry-url",
                    EnvironmentVariables = new[] { new KeyValuePair<object, object>("key", "value") },
                    Ports = new[] { new KeyValuePair<object, object>("8080", "80") }

                },
                Hosts = new Host[]
                {
                    new Host()
                    {
                        Ip = "192.168.99.1",
                        Username = "root",
                        Password = "r00t"
                    },
                }
            };

            var applicationSetupHolder = await _applicationDao.SaveApplicationSetup(expectedApplication);

            Assert.NotNull(applicationSetupHolder);

            var actualApplication = GetApplicationById(applicationSetupHolder.AsEnumerable().First().Id).Result;

            Assert.NotNull(actualApplication.Id);
            Assert.AreEqual(actualApplication.Name, expectedApplication.Name);
            //Assert.AreEqual(actualApplication.DockerSetup.ImageName, expectedApplication.DockerSetup.ImageName);
            //Assert.AreEqual(actualApplication.Hosts, expectedApplication.Hosts);
        }

        private async Task<Application> CreateApplication(string name)
        {
            var application = new Application
            {
                Name = name
            };

            var collection = GetCollection<Application>("applications");
            await collection.InsertOneAsync(application);

            return application;
        }

        public async Task<Application> GetApplicationById(ObjectId id)
        {
            var collection = GetCollection<Application>("applications");
            var cursor = await collection.FindAsync(application => application.Id.Equals(id));

            return await cursor.FirstOrDefaultAsync();
        }
    }
}