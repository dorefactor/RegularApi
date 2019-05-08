using System.Collections.Generic;

namespace RegularApi.Domain.Model
{
    public class HostSetup
    {
        public string Tag { get; set; }

        public IList<Host> Hosts { get; set; }
    }
}