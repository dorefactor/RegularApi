using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RegularApi.Dao.Model
{
    public class Application
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public DockerSetup DockerSetup { get; set; }
        public IList<HostSetup> HostsSetup { get; set; }
    }
}