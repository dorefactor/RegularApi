using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace RegularApi.Domain.Views
{
    public class DeploymentTemplateView
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ApplicationId { get; set; }

        [Required]
        [JsonProperty(PropertyName = "applicationSetup")]
        public ApplicationSetupView ApplicationSetupView { get; set; }

        [Required]
        [JsonProperty(PropertyName = "hostsSetup")]
        public IList<HostSetupView> HostSetupViews { get; set; }
    }
}