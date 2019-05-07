using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RegularApi.Domain.Views
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DeploymentOrderView
    {
        public string DeploymentTemplateId { get; set; }

        public string RequestId { get; set; }

        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "applicationSetup")]
        public ApplicationSetupView ApplicationSetupView { get; set; }

        [Required]
        [JsonProperty(PropertyName = "hostsSetup")]
        public IList<HostSetupView> HostSetupViews { get; set; }
    }
}
