using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DoRefactor.AspNetCore.DataProtection.Model
{
    public sealed class MongoStoredKey
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonRequired]
        public string Xml { get; set; }

        public string FriendlyName { get; set; }
    }
}