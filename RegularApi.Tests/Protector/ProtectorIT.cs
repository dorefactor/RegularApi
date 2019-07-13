using DoRefactor.AspNetCore.DataProtection.Protector;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace RegularApi.Tests.Protector
{
    public class ProtectorIT : BaseIT
    {
        private const string TextToProtect = "something to protect";
        private IProtector _protector;

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            CreateTestServer();
            
            _protector = ServiceProvider.GetRequiredService<IProtector>();
        }

        [TearDown]
        public void TearDown()
        {
            ReleaseMongoDbServer();
        }

        [Test]
        public void TestProtect()
        {
            var protectedText = _protector.ProtectObject(TextToProtect);

            protectedText.Should().NotBeNullOrEmpty();
            TextToProtect.Should().NotBe(protectedText);
        }

        [Test]
        public void TestUnprotect()
        {
            var protectedText = _protector.ProtectObject(TextToProtect);

            protectedText.Should().NotBeNullOrEmpty();

            var unprotectedText = _protector.UnprotectObject(protectedText);

            unprotectedText.Should().NotBeNullOrEmpty();
            unprotectedText.Should().Be(TextToProtect);
        }
    }
}