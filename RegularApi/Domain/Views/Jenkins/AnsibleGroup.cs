using System.Collections.Generic;
using Newtonsoft.Json;

namespace RegularApi.Domain.Views.Jenkins
{
    public class AnsibleGroup
    {
        public string Name { get; } = "all";

        [JsonProperty(PropertyName = "hosts")]
        public IList<AnsibleHost> AnsibleHosts { get; set; }

        public ApplicationSetup ApplicationSetup { get; set; }
    }
}
