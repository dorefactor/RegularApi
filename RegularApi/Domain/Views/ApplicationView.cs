using Newtonsoft.Json;

namespace RegularApi.Domain.Views
{
    public class ApplicationView
    {
        public string Name { get; set; }

        [JsonProperty(PropertyName = "applicationSetup")]
        public ApplicationSetupView ApplicationSetupView { get; set; }
    }
}