using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RegularApi.Domain.Model;
using RegularApi.Domain.Views;
using RegularApi.Enums;
using RegularApi.Tests.Fixtures;
using RegularApi.Transformers;

namespace RegularApi.Tests.Controllers.Configuration
{
    public class ApplicationsControllerIT : BaseControllerIT
    {
        private const string ApplicationUri = "/configuration/applications";

        private DaoFixture _daoFixture;
        private ITransformer<ApplicationView, Application> _applicationTransformer;

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();

            _daoFixture = ServiceProvider.GetRequiredService<DaoFixture>();
            _applicationTransformer = ServiceProvider.GetRequiredService<ITransformer<ApplicationView, Application>>();
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

        [Test]
        public async Task TestGetAllAsync_Ok()
        {
            var application = await _daoFixture.CreateApplicationAsync("test", ApplicationType.Docker);

            var responseMessage = await PerformGetAsync(ApplicationUri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            var expectedView = _applicationTransformer.Transform(application);
            var actualView = await GetResponseView<IList<ApplicationView>>(responseMessage);

            actualView.Should().BeEquivalentTo(expectedView);
        }

        [Test]
        public async Task TestGetAllAsync_EmptyOk()
        {
            var responseMessage = await PerformGetAsync(ApplicationUri);

            responseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

            var actualView = await GetResponseView<IList<ApplicationView>>(responseMessage);

            actualView.Should().BeEmpty();
        }
    }
}