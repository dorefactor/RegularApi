using Newtonsoft.Json;
using RegularApi.Converters;

namespace RegularApi.Domain.Views
{
    [JsonConverter(typeof(ApplicationSetupConverter))]
    public abstract class ApplicationSetupView
    {
        public string Type { get; set; }
    }
}
