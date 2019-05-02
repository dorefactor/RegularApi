using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RegularApi.Domain.Views;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Controllers.Configuration
{
    public class ApplicationsControllerIT : BaseControllerIT
    {
        private const string ApplicationUri = "/configuration/applications";

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public async Task TestNewApplicationSetupAsync_Created()
        {
            var applicationResource = ViewFixture.BuildApplicationView();
            var responseMessage = await PerformPostAsync(applicationResource, ApplicationUri);

            Assert.AreEqual(HttpStatusCode.OK, responseMessage.StatusCode);
        }
    }
}