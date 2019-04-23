using System.Collections.Generic;

namespace RegularApi.Controllers.Dashboard.Models
{
    public class DockerSetupResource
    {
        public string RegistryUrl { get; set; }
        public string ImageName { get; set; }
        public IList<KeyValuePair<object, object>> EnvironmentVariables { get; set; }
        public IList<KeyValuePair<object, object>> Ports { get; set; }
    }
}