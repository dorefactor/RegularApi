using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using RegularApi.Domain.Views;

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
        public async Task TestNewAsync_Ok()
        {
            var applicationView = GetPayloadViewFromJsonFile<ApplicationView>("../../../Samples/Controllers/Payloads/application.json");
            var responseMessage = await PerformPostAsync(applicationView, ApplicationUri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            var actualResponse = await GetResponseView<NewResourceResponseView>(responseMessage);

            actualResponse.Link.Should().MatchRegex(ApplicationUri + "/\\b");
        }
    }
}