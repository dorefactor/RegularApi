using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace RegularApi.Domain.Model
{
    public class DeploymentOrder
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
        public ObjectId DeploymentTemplateId { get; set; }
        public string ApplicationVersion { get; set; }
        public IList<HostSetup> HostsSetup { get; set; }
    }
}