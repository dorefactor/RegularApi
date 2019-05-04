using System.Collections.Generic;

namespace RegularApi.Domain.Model
{
    public class DeploymentOrderDetailVo
    {
        public string ApplicationName { get; set; }
        public string ApplicationVersion { get; set; }
        public DockerSetup DockerSetup { get; set; }
        public IList<HostSetup> HostsSetup { get; set; }
    }
}
