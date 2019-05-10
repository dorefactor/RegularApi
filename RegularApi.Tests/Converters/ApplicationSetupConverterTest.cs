using System;
using System.IO;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RegularApi.Domain.Views;
using RegularApi.Domain.Views.Docker;
using RegularApi.Enums;
using RegularApi.Tests.Fixtures;

namespace RegularApi.Converters
{
    public class ApplicationSetupConverterTest
    {
        [Datapoints]
        public string[] DateTimeFormatPattern = { "yyyy-MM-dd", "yyyy/MM/dd HH:mm" };

        private Mock<JsonWriter> _jsonWriter;
        private Mock<JsonSerializer> _jsonSerializer;
        private Mock<JsonReader> _jsonReader;

        private ApplicationSetupConverter _applicationSetupConverter;

        [SetUp]
        public void SetUp()
        {
            _jsonWriter = new Mock<JsonWriter>();
            _jsonSerializer = new Mock<JsonSerializer>();
            _jsonReader = new Mock<JsonReader>();

            _applicationSetupConverter = new ApplicationSetupConverter();
        }

        [Test]
        public void TestCanWrite()
        {
            var canWrite = _applicationSetupConverter.CanWrite;

            canWrite.Should().BeFalse();
        }

        [Test]
        public void TestCanRead()
        {
            var canRead = _applicationSetupConverter.CanRead;

            canRead.Should().BeTrue();
        }

        [Test]
        public void TestCanConvert_True()
        {
            var canConvert = _applicationSetupConverter.CanConvert(
                new DockerApplicationSetupView().GetType().BaseType);

            canConvert.Should().BeTrue();
        }

        [Test]
        public void TestCanConvert_False()
        {
            var canConvert = _applicationSetupConverter.CanConvert(
                new ApplicationView().GetType().BaseType);

            canConvert.Should().BeFalse();
        }

        [Test]
        public void TestWriteJson_ReturnInvalidOperationException()
        {
            var objectToSerialize = new Object();

            Assert.Throws<InvalidOperationException>(() =>
                             _applicationSetupConverter.WriteJson(_jsonWriter.Object,
                                                                  objectToSerialize,
                                                                  _jsonSerializer.Object));
        }

        [Test]
        public void TestReadJson()
        {
            var jsonReader = new JsonTextReader(new StreamReader("../../../Samples/Controllers/Payloads/application-setup.json"));
            var jsonSerializer = new JsonSerializer();

            var actualApplicationSetup = _applicationSetupConverter.ReadJson(jsonReader,
                                                                             new DockerApplicationSetupView().GetType().BaseType,
                                                                             new Object(),
                                                                             jsonSerializer);

            var expectedApplicationSetup = ViewFixture.BuildDockerApplicationSetupView(ApplicationType.Docker.ToString());

            actualApplicationSetup.Should().BeEquivalentTo(expectedApplicationSetup);
        }
    }
}
