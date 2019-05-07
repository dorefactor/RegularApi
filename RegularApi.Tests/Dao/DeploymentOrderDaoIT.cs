using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RegularApi.Dao;
using RegularApi.Enums;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Dao
{
    public class DeploymentOrderDaoIT : BaseDaoIT
    {
        private IDeploymentOrderDao _deploymentOrderDao;
        private DaoFixture _daoFixture;

        [SetUp]
        public void Setup()
        {
            CreateMongoDbServer();
            CreateTestServer();
            DropCollection(DeploymentOrderDao.DeploymentOrderCollectionName);

            _deploymentOrderDao = GetDao<IDeploymentOrderDao>();
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
            var deploymentOrder = ModelFixture.BuildDeploymentOrder("request-id");

            var actualDeploymentOrder = await _deploymentOrderDao.SaveAsync(deploymentOrder);
            var expectedDeploymentOrder = await _daoFixture.GetDeploymentOrderByIdAsync(deploymentOrder.Id.ToString());

            actualDeploymentOrder.Should().NotBeNull();
            actualDeploymentOrder.Id.Should().BeEquivalentTo(expectedDeploymentOrder.Id);
            actualDeploymentOrder.RequestId.Should().BeEquivalentTo(expectedDeploymentOrder.RequestId);
        }

        [Test]
        public async Task TestGetByRequestIdAsync()
        {
            var requestId = "request-id";
            var expectedDeploymentOrder = await _daoFixture.CreateDeploymentOrderAsync(requestId, ApplicationType.Docker);

            var deploymentOrderHolder = await _deploymentOrderDao.GetByRequestIdAsync(requestId);

            deploymentOrderHolder.IsSome.Should().BeTrue();

            var actualDeploymentOrder = deploymentOrderHolder.AsEnumerable().First();

            actualDeploymentOrder.Id.Should().BeEquivalentTo(expectedDeploymentOrder.Id);
        }
    }
}