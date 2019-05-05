using System.Collections.Generic;

namespace RegularApi.Domain.Views
{
    public class HostSetupView
    {
        public string Tag { get; set; }

        public IList<HostView> Hosts { get; set; }
    }
}