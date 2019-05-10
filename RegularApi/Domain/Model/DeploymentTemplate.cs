using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RegularApi.Domain.Model
{
    public class DeploymentTemplate
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id;

        public string Name { get; set; }

        public Application Application { get; set; }

        public IList<HostSetup> HostsSetup { get; set; }
    }
}