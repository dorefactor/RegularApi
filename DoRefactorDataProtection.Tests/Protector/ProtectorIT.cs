using DoRefactor.AspNetCore.DataProtection;
using DoRefactor.AspNetCore.DataProtection.Protector;
using DoRefactor.Tests.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace MongoDataProtection.Test.Protector
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
            
        }
    }
}