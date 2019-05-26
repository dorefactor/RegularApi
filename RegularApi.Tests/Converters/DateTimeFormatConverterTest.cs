using FluentAssertions;
using NUnit.Framework;
using RegularApi.Converters;

namespace RegularApi.Tests.Converters
{
    public class DateTimeFormatConverterTest
    {
        [Datapoints]
        public string[] DateTimeFormatPattern = { "yyyy-MM-dd", "yyyy/MM/dd HH:mm" };

        private DateTimeFormatConverter _dateTimeFormatConverter;

        [Theory]
        public void TestDateTimeFormat(string dateFormat)
        {
            _dateTimeFormatConverter = new DateTimeFormatConverter(dateFormat);

            _dateTimeFormatConverter.DateTimeFormat.Should().BeSameAs(dateFormat);
        }
    }
}
