using Newtonsoft.Json;
using RegularApi.Converters;

namespace RegularApi.Domain.Views
{
    [JsonConverter(typeof(ApplicationSetupConverter))]
    public abstract class ApplicationSetupView
    {
        public virtual string Type { get; set; }
    }
}
