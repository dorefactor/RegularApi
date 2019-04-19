using System.Collections.Generic;
using Newtonsoft.Json;

namespace RegularApi.Controllers.Dashboard.Models
{
    public class ApplicationResource
    {
        public string Name { get; set; }
        [JsonProperty(PropertyName = "DockerSetup")]
        public DockerSetupResource DockerSetupResource { get; set; }

        [JsonProperty(PropertyName = "Hosts")]
        public IList<HostResource> HostResources { get; set; }
    }
}