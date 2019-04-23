using System.Collections.Generic;
using Newtonsoft.Json;

namespace RegularApi.Controllers.Dashboard.Models
{
    public class HostSetupResource
    {
        public string TagName { get; set; }
        public IList<HostResource> HostsResource { get; set; }
    }
}