using System.Collections.Generic;

namespace RegularApi.Dao.Model
{
    public class HostSetup
    {
        public string TagName { get; set; }
        public IList<Host> Hosts { get; set; }
    }
}