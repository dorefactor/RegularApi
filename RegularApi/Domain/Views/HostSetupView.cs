using System.Collections.Generic;
using Newtonsoft.Json;

namespace RegularApi.Domain.Views
{
    public class HostSetupView
    {
        public string Tag { get; set; }

        [JsonProperty(PropertyName = "hosts")]
        public IList<HostView> HostViews { get; set; }
    }
}