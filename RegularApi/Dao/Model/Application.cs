using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RegularApi.Dao.Model
{
    public class Application
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}