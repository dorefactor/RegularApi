using NUnit.Framework;
using RegularApi.Dao;

namespace RegularApi.Tests.Dao
{
    public class ApplicationDaoTest : IntegrationTestBase
    {
        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
        }

        [Test]
        public void test()
        {
            var dao = (IApplicationDao) ServiceProvider.GetService(typeof(IApplicationDao));
            
            Assert.NotNull(dao);

            var apps = dao.GetApplications();
        }
    }
}