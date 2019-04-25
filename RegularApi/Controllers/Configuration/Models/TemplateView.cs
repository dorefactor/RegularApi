using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RegularApi.Controllers.Configuration.Models
{
    public class TemplateView
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string ApplicationId { get; set; }
        
        [Required]
        public IList<HostSetupView> HostsSetup { get; set; }
                
        [Required]
        public IList<KeyValuePair<object, object>> Ports { get; set; }
        
        public IList<KeyValuePair<object, object>> EnvironmentVariables { get; set; }        
    }
}