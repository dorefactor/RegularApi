using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RegularApi.Domain.Views
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DeploymentOrderView
    {
        [Required]
        public string DeploymentTemplateId { get; set; }

        [JsonProperty(PropertyName = "applicationSetup")]
        public ApplicationSetupView ApplicationSetupView { get; set; }

        [Required]
        [JsonProperty(PropertyName = "hostsSetup")]
        public IList<HostSetupView> HostSetupViews { get; set; }
    }
}
