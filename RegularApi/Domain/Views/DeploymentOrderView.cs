using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using RegularApi.Converters;

namespace RegularApi.Domain.Views
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class DeploymentOrderView
    {
        public string DeploymentTemplateId { get; set; }

        [JsonProperty(Required = Required.Default)]
        public string RequestId { get; set; }

        [JsonConverter(typeof(DateTimeFormatConverter), "yyyy-MM-dd HH:mm:ss")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "application")]
        public ApplicationView ApplicationView { get; set; }

        [Required]
        [JsonProperty(PropertyName = "hostsSetup")]
        public IList<HostSetupView> HostSetupViews { get; set; }
    }
}
