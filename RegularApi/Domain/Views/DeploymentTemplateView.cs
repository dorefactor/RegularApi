using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RegularApi.Domain.Views
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DeploymentTemplateView
    {
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [JsonProperty(PropertyName = "application")]
        public ApplicationView ApplicationView { get; set; }

        [Required]
        [JsonProperty(PropertyName = "hostsSetup")]
        public IList<HostSetupView> HostSetupViews { get; set; }
    }
}