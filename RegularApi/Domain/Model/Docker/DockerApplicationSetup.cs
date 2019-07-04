using System.Collections.Generic;

namespace RegularApi.Domain.Model.Docker
{
    public class DockerApplicationSetup : ApplicationSetup
    {
        public Registry Registry { get; set; }

        public Image Image { get; set; }

        public IDictionary<string, string> Ports { get; set; }

        public IDictionary<string, string> EnvironmentVariables { get; set; }

        public IDictionary<string, string> ExtraHosts { get; set; }

        public IDictionary<string, string> Volumes { get; set; }
    }
}
