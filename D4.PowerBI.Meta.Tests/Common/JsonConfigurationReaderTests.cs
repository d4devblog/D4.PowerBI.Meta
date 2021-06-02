using D4.PowerBI.Meta.Common;
using D4.PowerBI.Meta.Constants;
using FluentAssertions;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace D4.PowerBI.Meta.Tests.Common
{
    public class JsonConfigTestObject
    {
        public string? config { get; set; }
    }

    public class JsonConfigurationReaderTests
    {
        [Theory]
        [InlineData("{}")]
        [InlineData("[]")]
        [InlineData("{\"emptyString\": \"\"}")]
        [InlineData("{\"emptyObject\": {}}")]
        [InlineData("{\"emptyArray\": []}")]
        public void WHEN_json_configuration_is_effectively_empty_THEN_empty_list_returned(
            string json)
        {
            var document = JsonDocument.Parse(json);

            var properties = JsonConfigurationReader.ReadPropertyConfigurationNode(document.RootElement);
            properties.Should().NotBeNull();
            properties.Count().Should().Be(0);
        }

        [Theory]
        [InlineData("{ \"stringProperty\": \"value\" }", "stringProperty", "value")]
        [InlineData("{ \"numericProperty\": 123 }", "numericProperty", 123)]
        [InlineData("{ \"decimalProperty\": 123.1 }", "decimalProperty", 123.1)]
        [InlineData("{ \"booleanProperty\": true }", "booleanProperty", true)]
        [InlineData("{ \"booleanProperty\": false }", "booleanProperty", false)]
        [InlineData("{ \"stringArrayProperty\": [\"abc\", \"def\"] }", "stringArrayProperty", new object[2] { "abc", "def" })]
        [InlineData("{ \"numericArrayProperty\": [1,2,3] }", "numericArrayProperty", new object[3] { 1, 2, 3 })]
        public void WHEN_json_contains_single_properties_THEN_list_contains_expected_values(
            string rawJson, string propertyName, object value)
        {
            var config = new JsonConfigTestObject
            {
                config = rawJson
            };

            var document = JsonDocument.Parse(JsonSerializer.Serialize(config));
            var configElement = document.RootElement.GetProperty(ReportLayoutDocument.Configuration);

            var properties = JsonConfigurationReader.ReadPropertyConfigurationNode(configElement);
            properties.Should().NotBeNull();
            properties.Count().Should().Be(1);

            properties[0].Name.Should().Be(propertyName);
            properties[0].Value.Should().BeEquivalentTo(value);
            properties[0].Raw.Should().NotBeEmpty();
            properties[0].ChildProperties.Should().BeEmpty("JSON Properties dont have child values");
        }
    }
}
