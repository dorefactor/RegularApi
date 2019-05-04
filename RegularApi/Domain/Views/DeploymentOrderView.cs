using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RegularApi.Domain.Views
{
    public class DeploymentOrderView
    {
        [Required]
        public string DeploymentTemplateId { get; set; }

        [Required]
        public string Version { get; set; }

        [Required]
        [JsonProperty(PropertyName = "Hosts")]
        public IList<HostSetupView> HostSetupViews { get; set; }
    }
}
