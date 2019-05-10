using Newtonsoft.Json.Converters;

namespace RegularApi.Converters
{
    public class DateTimeFormatConverter : IsoDateTimeConverter
    {
        public DateTimeFormatConverter(string dateFormat)
        {
            DateTimeFormat = dateFormat;
        }
    }
}
