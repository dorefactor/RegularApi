using System.Collections.Generic;
using MongoDB.Bson;

namespace RegularApi.Domain.Model
{
    public class DeploymentOrderVo
    {
        public ObjectId Id { get; set; }

        public string RequestId { get; set; }

        public ApplicationSetup ApplicationSetupFromApplication { get; set; }

        public ApplicationSetup ApplicationSetupFromDeploymentTemplate { get; set; }

        public ApplicationSetup ApplicationSetupFromDeploymentOrder { get; set; }

        public IList<HostSetup> HostsSetupFromDeploymentTemplate { get; set; }

        public IList<HostSetup> HostsSetupFromDeploymentOrder { get; set; }
    }
}
