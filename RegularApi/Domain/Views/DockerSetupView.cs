using System.Collections.Generic;

namespace RegularApi.Domain.Views
{
    public class DockerSetupView
    {
        public string RegistryUrl { get; set; }
        public string ImageName { get; set; }
        public IList<KeyValuePair<object, object>> EnvironmentVariables { get; set; }
        public IList<KeyValuePair<object, object>> Ports { get; set; }
    }
}