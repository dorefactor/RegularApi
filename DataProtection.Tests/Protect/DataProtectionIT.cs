using DoRefactor.AspNetCore.DataProtection;
using DoRefactor.Tests.AspNetCore.DataProtection;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataProtection.Tests.Protect
{
    public class DataProtectionIT : BaseIT
    {
        private IXmlRepository _xmlRepository;
        private IDataProtectionProvider _protectionProvider;

        [SetUp]
        public void Setup()
        {
            CreateTestServer();
            _xmlRepository = ServiceProvider.GetRequiredService<IXmlRepository>();
            _protectionProvider = ServiceProvider.GetRequiredService<IDataProtectionProvider>();
        }

        [TearDown]
        public void TearDown()
        {
            BaseDatabase.ReleaseMongoDbServer();
        }

        [Test]
        public void TestDataProtectionConfiguration()
        {
            var protector = _protectionProvider.CreateProtector("testScope");
            var text = "some text";
            var protectedText = protector.Protect(text);

            var result = protector.Unprotect(protectedText);

            result.Should().Be(text);

            var keys = _xmlRepository.GetAllElements();

            keys.Should().HaveCountGreaterOrEqualTo(1);
        }
    }
}