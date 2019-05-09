using Newtonsoft.Json;

namespace RegularApi.Domain.Views
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ApplicationView
    {
        public string Id { get; set; }

        public string Name { get; set; }

        [JsonProperty(PropertyName = "applicationSetup")]
        public ApplicationSetupView ApplicationSetupView { get; set; }
    }
}