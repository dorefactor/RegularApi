using System.Collections.Generic;

namespace RegularApi.Domain.Views.Jenkins
{
    public class AnsibleHost
    {
        public string PublicIp { get; set; }
        public Dictionary<string, string> Variables { get; set; }
    }
}
