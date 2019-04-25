using System.Collections.Generic;

namespace RegularApi.Controllers.Configuration.Models
{
    public class HostSetupView
    {
        public string TagName { get; set; }

        public IList<HostView> Hosts { get; set; }
    }
}