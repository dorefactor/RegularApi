using Newtonsoft.Json;

namespace RegularApi.Controllers.Dashboard.Models
{
    public class HostResource
    {
        [JsonProperty(PropertyName = "Value1")]
        public string HostIp { get; set; }

        [JsonProperty(PropertyName = "Value2")]
        public string Username { get; set; }

        [JsonProperty(PropertyName = "Value3")]
        public string Password { get; set; }
    }
}