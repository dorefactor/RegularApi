using NUnit.Framework;
using RegularApi.Controllers.Deployment.Validators;
using System.Linq;
using System.Xml.Linq;
using RegularApi.Controllers.Deployment.Views;
using RegularApi.Dao.Model;

namespace RegularApi.Tests.Controllers.Deployment.Validators
{
    public class DeploymentRequestValidatorTest
    {
        private DeploymentRequestValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new DeploymentRequestValidator();
        }

        [Test]
        public void TestWhenNullReturnError()
        {
            var result = _validator.Validate(null);

            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.Contains("Application cannot be null"));
        }

        [Test]
        public void TestWhenFieldsAreNullReturnError()
        {
            var application = new ApplicationRequest();

            
//            var result = _validator.Validate(application);

/*
            Assert.IsNotEmpty(result);
            Assert.IsTrue(result.Contains("Name is required"));
            Assert.IsTrue(result.Contains("Tag is required"));
*/
        }
    }
}