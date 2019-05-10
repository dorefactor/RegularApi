using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RegularApi.Domain.Views;
using RegularApi.Domain.Views.Docker;
using RegularApi.Enums;

namespace RegularApi.Converters
{
    public class ApplicationSetupConverter : JsonConverter
    {
        public override bool CanWrite => false;
        public override bool CanRead => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(ApplicationSetupView);
        }

        public override void WriteJson(JsonWriter writer,
                                       object value, 
                                       JsonSerializer serializer)
        {
            throw new InvalidOperationException("Use default serialization.");
        }

        public override object ReadJson(JsonReader reader, 
                                        Type objectType, 
                                        object existingValue, 
                                        JsonSerializer serializer)
        {

            var jsonObject = JObject.Load(reader);
            var applicationSetup = default(ApplicationSetupView);
            var applicationType = jsonObject.Value<string>("type");
            var applicationTypeEnum = (ApplicationType)Enum.Parse(typeof(ApplicationType), applicationType);

            switch (applicationTypeEnum)
            {
                case ApplicationType.Docker:
                    applicationSetup = new DockerApplicationSetupView();
                    break;
            }

            serializer.Populate(jsonObject.CreateReader(), applicationSetup);

            return applicationSetup;
        }
    }
}
