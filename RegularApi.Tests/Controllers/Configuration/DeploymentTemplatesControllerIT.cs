using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Controllers.Configuration
{
    public class DeploymentTemplatesControllerIT : BaseControllerIT
    {
        private const string TemplatesUri = "/configuration/deploymentTemplates";

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
        public async Task CreateDeploymentTemplateTest()
        {
            var templateName = "super-template";
            var deploymentTemplateView = ViewFactory.BuildDeploymentTemplateView(templateName);

            var response = await PerformPostAsync(deploymentTemplateView, TemplatesUri);
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}