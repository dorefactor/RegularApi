using MongoDB.Bson.Serialization.Attributes;
using RegularApi.Domain.Model.Docker;
using RegularApi.Enums;

namespace RegularApi.Domain.Model
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(DockerApplicationSetup))]
    public abstract class ApplicationSetup
    {
        public ApplicationType ApplicationType { get; set; }
    }
}
