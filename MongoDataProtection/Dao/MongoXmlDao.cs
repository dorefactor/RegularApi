using System.Collections.Generic;
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
            throw new System.NotImplementedException();
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            throw new System.NotImplementedException();
        }
    }
}