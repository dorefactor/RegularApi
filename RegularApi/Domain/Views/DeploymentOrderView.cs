using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RegularApi.Domain.Views
{
    public class DeploymentOrderRequestView
    {
        [Required]
        public string DeploymentTemplateId { get; set; }

        [JsonProperty(PropertyName = "ApplicationSetup")]
        public ApplicationSetupView ApplicationSetupView { get; set; }

        [Required]
        [JsonProperty(PropertyName = "HostsSetup")]
        public IList<HostSetupView> HostSetupViews { get; set; }
    }
}
