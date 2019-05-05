using System.Collections.Generic;
using Newtonsoft.Json;

namespace RegularApi.Domain.Views.Docker
{
    public class DockerApplicationSetupView : ApplicationSetupView
    {
        [JsonProperty(PropertyName = "registry")]
        public RegistryView RegistryView { get; set; }

        [JsonProperty(PropertyName = "image")]
        public ImageView ImageView { get; set; }

        public IDictionary<string, string> Ports { get; set; }

        public IDictionary<string, string> EnvironmentVariables { get; set; }
    }
}