using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RegularApi.Dao.Model
{
    public class Application
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public DockerSetup DockerSetup { get; set; }
        public IList<Host> Hosts { get; set; }
    }
}