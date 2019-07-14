using DataProtection.Protectors;
using DataProtection.Tests.Fixture;
using DoRefactor.AspNetCore.DataProtection;
using DoRefactor.Tests.AspNetCore.DataProtection;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataProtection.Tests.Protector
{
    public class ProtectorIT : BaseIT
    {
        private IProtector _protector;
        
        [SetUp]
        public void SetUp()
        {
            CreateTestServer();
            _protector = ServiceProvider.GetRequiredService<IProtector>();
        }

        [TearDown]
        public void TearDown()
        {
            BaseDatabase.ReleaseMongoDbServer();            
        }

        [Test]
        public void TestObjectProtection()
        {
            var dummyObject = DummyObjectFixture.BuildDummyObject("some text");
            var protectedObject = _protector.ProtectObject(dummyObject);

            protectedObject.Should().NotBeNull()
                .And.NotBe(dummyObject);
            
            protectedObject.Text.Should().NotBe("some text");
        }

        [Test]
        public void TestObjectUnprotect()
        {
            var dummyObject = DummyObjectFixture.BuildDummyObject("some text");
            var protectedObject = _protector.ProtectObject(dummyObject);
            var unprotectedObject = _protector.UnprotectObject(protectedObject);

            unprotectedObject.Should().NotBeNull()
                .And.BeEquivalentTo(dummyObject);
        }

        [Test]
        public void TestProtectText()
        {
            var text = "something";
            var protectedText = _protector.ProtectText(text);

            protectedText.Should().NotBeNull()
                .And.NotBe(text);
        }

        [Test]
        public void TestUnprotectText()
        {
            var text = "something";
            var protectedText = _protector.ProtectText(text);

            var unprotectedText = _protector.UnprotectText(protectedText);

            unprotectedText.Should().NotBeNull()
                .And.Be(text);
        }
    }
}