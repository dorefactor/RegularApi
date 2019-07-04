using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DoRefactor.AspNetCore.DataProtection.Model;
using Microsoft.AspNetCore.DataProtection.Repositories;
using MongoDB.Driver;

namespace DoRefactor.AspNetCore.DataProtection.Repository
{
    public sealed class MongoXmlRepository : IXmlRepository
    {
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<MongoStoredKey> _keyCollection;

        public MongoXmlRepository(IMongoDatabase db, string collectionName)
        {
            _db = db;
            _keyCollection = db.GetCollection<MongoStoredKey>(collectionName);
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var keys = _keyCollection.Find(FilterDefinition<MongoStoredKey>.Empty).ToList();

            return keys.Select(key => XElement.Parse(key.Xml))
                .ToList()
                .AsReadOnly();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            var storedKey = new MongoStoredKey
            {
                FriendlyName = friendlyName,
                Xml = element.ToString(SaveOptions.DisableFormatting)
            };

            _keyCollection.InsertOne(storedKey);
        }
    }
}