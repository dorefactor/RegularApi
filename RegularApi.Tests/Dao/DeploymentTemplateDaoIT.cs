using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Dao
{
    public class DeploymentTemplateDaoIT : BaseDaoIT
    {
        private IDeploymentTemplateDao _templateDao;

        [SetUp]
        public void Setup()
        {
            CreateMongoDbServer();
            CreateTestServer();
            DropCollection("deploymentTemplates");

            _templateDao = GetDao<IDeploymentTemplateDao>();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public async Task AddNewTemplateTest()
        {
            var template = ModelFactory.BuildDeploymentTemplate("super-template");

            var storedTemplate = await _templateDao.NewAsync(template);
            var expectedTemplate = await GetDaoFixture().GetDeploymentTemplateByIdAsync(template.Id);

            storedTemplate.Should().NotBeNull();
            expectedTemplate.Should().BeEquivalentTo(storedTemplate);
        }

        [Test]
        public async Task GetDeploymentTemplateByNameTest()
        {
            const string templateName = "deployment-template";
            var expectedTemplate = await GetDaoFixture().CreateDeploymentTemplateAsync(templateName);

            var holder = await _templateDao.GetByNameAsync(templateName);
            
            Assert.True(holder.IsSome);
            var storedTemplate = holder.AsEnumerable().First();
            
            expectedTemplate.Should().BeEquivalentTo(storedTemplate);
        }
    }
}