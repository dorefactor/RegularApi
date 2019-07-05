using DoRefactor.AspNetCore.DataProtection.Model;
using DoRefactor.AspNetCore.DataProtection.Repository;
using Microsoft.AspNetCore.DataProtection.Repositories;
using MongoDB.Driver;
using NUnit.Framework;
using FluentAssertions;
using System.Xml.Linq;
using System.Linq;

namespace DoRefactor.Tests.AspNetCore.DataProtection.Repository
{
    public class MongoXmlRepositoryTest : BaseDatabase
    {
        private IXmlRepository _xmlRepository;

        [SetUp]
        public void SetUp()
        {
            CreateMongoDbServer();
            _xmlRepository = new MongoXmlRepository(MongoClient.GetDatabase(DatabaseName), CollectionName);
        }

        [TearDown]
        public void TearDown()
        {
            MongoClient.DropDatabase(DatabaseName);
            ReleaseMongoDbServer();
        }

        [Test]
        public void TestGetAllElements()
        {
            var elementOne = addKey("test-key");
            var elementTwo = addKey("other-key");

            var elements = _xmlRepository.GetAllElements();

            elements.Should().HaveCount(2)
                .And.BeEquivalentTo(elementOne, elementTwo);
        }

        [Test]
        public void TestStoreElement()
        {
            var xml = "<test></test>";

            var element = XElement.Parse(xml);
            var name = "test-element";

            _xmlRepository.StoreElement(element, name);

            var storedKey = getStoredKey(name);

            storedKey.Should().NotBeNull();
            storedKey.FriendlyName.Should().Be(name);
            storedKey.Xml.Should().Be(xml);
        }

        private XElement addKey(string name)
        {
            var _keyCollection = GetKeysCollection();

            var key = new MongoStoredKey
            {
                FriendlyName = name,
                Xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?><test></test>"
            };

            _keyCollection.InsertOne(key);

            return XElement.Parse(key.Xml);
        }

        private MongoStoredKey getStoredKey(string name)
        {
            var filter = new FilterDefinitionBuilder<MongoStoredKey>().Where(key => key.FriendlyName.Equals(name));
            return GetKeysCollection().Find(filter).FirstOrDefault();
        }

        private IMongoCollection<MongoStoredKey> GetKeysCollection()
        {
            return MongoClient.GetDatabase(DatabaseName)
                .GetCollection<MongoStoredKey>(CollectionName);
        }
    }
}