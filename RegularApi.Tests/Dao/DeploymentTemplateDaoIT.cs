using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Enums;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Dao
{
    public class DeploymentTemplateDaoIT : BaseDaoIT
    {
        private IDeploymentTemplateDao _deploymentTemplateDao;
        private DaoFixture _daoFixture;

        [SetUp]
        public void Setup()
        {
            CreateMongoDbServer();
            CreateTestServer();
            DropCollection(DeploymentTemplateDao.CollectionName);

            _deploymentTemplateDao = GetDao<IDeploymentTemplateDao>();
            _daoFixture = GetDaoFixture();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public async Task TestSaveAsync()
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate("super-template", ApplicationType.Docker);

            var actualTemplate = await _deploymentTemplateDao.SaveAsync(deploymentTemplate);
            var expectedTemplate = await _daoFixture.GetDeploymentTemplateByIdAsync(deploymentTemplate.Id);

            actualTemplate.Should().NotBeNull();
            actualTemplate.Should().BeEquivalentTo(expectedTemplate);
        }

        [Test]
        public async Task TestGetByNameAsync()
        {
            string templateName = "deployment-template";
            var expectedTemplate = await _daoFixture.CreateDeploymentTemplateAsync(templateName, ApplicationType.Docker);

            var deploymentTemplateHolder = await _deploymentTemplateDao.GetByNameAsync(templateName);

            deploymentTemplateHolder.IsSome.Should().BeTrue();

            var actualTemplate = deploymentTemplateHolder.AsEnumerable().First();

            actualTemplate.Should().BeEquivalentTo(expectedTemplate);
        }

        [Test]
        public async Task TestGetAllAsync()
        {
            var expectedTemplate = await _daoFixture.CreateDeploymentTemplateAsync("test", ApplicationType.Docker);

            var deploymentTemplatesHolder = await _deploymentTemplateDao.GetAllAsync();

            deploymentTemplatesHolder.Should().NotBeEmpty();

            var actual = deploymentTemplatesHolder.First(_ => expectedTemplate.Id.Equals(_.Id));

            actual.Should().NotBeNull();

            actual.Should().BeEquivalentTo(expectedTemplate);
        }
    }
}