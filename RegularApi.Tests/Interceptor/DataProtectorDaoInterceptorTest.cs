using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RegularApi.Domain.Model;
using RegularApi.Enums;
using RegularApi.Interceptor;
using RegularApi.Protector;

using RegularApi.Tests.Fixtures;

namespace RegularApi.Tests.Interceptor
{
    public class DataProtectorDaoInterceptorTest: BaseIT

    {
        private const string AttributeValue = "something to be protected";

        private DataProtectorInterceptor _dataProtector;
        private IProtector _protector;
    
        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();
    
            _protector = ServiceProvider.GetRequiredService<IProtector>();
            _dataProtector = new DataProtectorInterceptor(_protector);
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public void TestAnnotatedAttributesAreProtected()
        {            
            var host = new Host
            {
                Ip = "127.0.0.1",
                Username = "user",
                Password = AttributeValue
            };

            var other = _dataProtector.ProtectAttributes(host);

            other.Password.Should().NotBe(AttributeValue);
        }

        [Test]
        public void blah()
        {
            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate("blah", ApplicationType.Docker);
            
            _dataProtector.ProtectOperation(deploymentTemplate, ProtectionOperationType.Protect);

            deploymentTemplate.Should().BeNull();
            // _dataProtector.PrintProperties(deploymentTemplate, 2);
        }
        
        [Test]
        public void TestAnnotatedAttributesAreUnprotected()
        {
//            var protectedValue = _protector.Protect(AttributeValue);
//            
//            var host = new Host
//            {
//                Ip = "localhost",
//                Username = "user",
//                Password = protectedValue
//            };
//
//            var other = _dataProtector.UnprotectAttributes(host);
//
//            other.Password.Should().Be(AttributeValue);

            var deploymentTemplate = ModelFixture.BuildDeploymentTemplate("blah", ApplicationType.Docker);

            var foo = _dataProtector.ProtectAttributes(deploymentTemplate);

            foo.Should().NotBeNull();
        }
            
    }
}