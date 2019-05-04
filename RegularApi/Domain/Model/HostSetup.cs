using System.Collections.Generic;

namespace RegularApi.Domain.Model
{
    public class HostSetup
    {
        public string TagName { get; set; }

        public IList<Host> Hosts { get; set; }
    }
}