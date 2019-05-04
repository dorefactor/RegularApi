using System.Collections.Generic;

namespace RegularApi.Domain.Model.Docker
{
    public class DockerSetup : ApplicationSetup
    {
        public Registry Registry { get; set; }

        public Image Image { get; set; }

        public string ContainerName { get; set; }

        public IDictionary<string, string> Ports { get; set; }

        public IDictionary<string, string> EnvironmentVariables { get; set; }
    }
}