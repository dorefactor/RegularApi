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
        [JsonProperty(PropertyName = "HostsSetup")]
        public IList<HostSetupView> HostsSetupViews { get; set; }
                
        [Required]
        public IList<KeyValuePair<object, object>> Ports { get; set; }
        
        public IList<KeyValuePair<object, object>> EnvironmentVariables { get; set; }        
    }
}