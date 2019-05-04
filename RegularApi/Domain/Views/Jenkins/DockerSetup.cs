using System.Collections.Generic;

namespace RegularApi.Domain.Views.Jenkins
{
    public class DockerSetup
    {
        public string Image { get; set; }

        public IList<string> Ports { get; set; }

        public IList<string> EnvironmentVariables { get; set; }
    }
}
