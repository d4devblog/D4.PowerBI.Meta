using D4.PowerBI.Meta.Common;
using D4.PowerBI.Meta.Constants;
using FluentAssertions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace D4.PowerBI.Meta.Tests.Common
{
    public class JsonConfigTestObject
    {
        [JsonPropertyName("config")]
        public string? Config { get; set; }
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
            properties.Should().HaveCount(0);
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
                Config = rawJson
            };

            var document = JsonDocument.Parse(JsonSerializer.Serialize(config));
            var configElement = document.RootElement.GetProperty(ReportLayoutDocument.Configuration);

            var properties = JsonConfigurationReader.ReadPropertyConfigurationNode(configElement);
            properties.Should().NotBeNull();
            properties.Should().HaveCount(1);

            properties[0].Name.Should().Be(propertyName);
            properties[0].Value.Should().BeEquivalentTo(value);
            properties[0].Raw.Should().NotBeEmpty();
            properties[0].ChildProperties.Should().BeEmpty("JSON Properties dont have child values");
        }

        [Fact]
        public void WHEN_json_contains_nested_properties_THEN_all_parent_child_properties_are_returned()
        {
            var rawJson = "{ \"parentObject\": {\"childStringProperty\": \"abc\", \"childNumericProperty\": 123} }";
            var config = new JsonConfigTestObject
            {
                Config = rawJson
            };

            var document = JsonDocument.Parse(JsonSerializer.Serialize(config));
            var configElement = document.RootElement.GetProperty(ReportLayoutDocument.Configuration);

            var properties = JsonConfigurationReader.ReadPropertyConfigurationNode(configElement);
            properties.Should().NotBeNull();
            properties.Should().HaveCount(1);

            properties[0].Name.Should().Be("parentObject");
            properties[0].Value.Should().BeNull();
            properties[0].Raw.Should().NotBeEmpty();
            properties[0].ChildProperties.Should().HaveCount(2);

            properties[0].ChildProperties[0].Name.Should().Be("childStringProperty");
            properties[0].ChildProperties[0].Value.Should().Be("abc");
            properties[0].ChildProperties[0].Raw.Should().NotBeEmpty();
            properties[0].ChildProperties[0].ChildProperties.Should().HaveCount(0);

            properties[0].ChildProperties[1].Name.Should().Be("childNumericProperty");
            properties[0].ChildProperties[1].Value.Should().Be(123);
            properties[0].ChildProperties[1].Raw.Should().NotBeEmpty();
            properties[0].ChildProperties[1].ChildProperties.Should().HaveCount(0);
        }

        [Fact]
        public void WHEN_object_contains_array_of_objects_THEN_all_child_properties_are_returned()
        {
            var rawJson = "{ \"parentObject\": [{\"childStringProperty\": \"abc\"}, {\"childStringProperty\": \"def\"}] }";
            var config = new JsonConfigTestObject
            {
                Config = rawJson
            };

            var document = JsonDocument.Parse(JsonSerializer.Serialize(config));
            var configElement = document.RootElement.GetProperty(ReportLayoutDocument.Configuration);

            var properties = JsonConfigurationReader.ReadPropertyConfigurationNode(configElement);
            properties.Should().NotBeNull();
            properties.Should().HaveCount(1);

            properties[0].Name.Should().Be("parentObject");
            properties[0].Value.Should().BeNull();
            properties[0].Raw.Should().NotBeEmpty();
            properties[0].ChildProperties.Should().HaveCount(2);

            properties[0].ChildProperties[0].Name.Should().Be("childStringProperty");
            properties[0].ChildProperties[0].Value.Should().Be("abc");
            properties[0].ChildProperties[0].Raw.Should().NotBeEmpty();
            properties[0].ChildProperties[0].ChildProperties.Should().HaveCount(0);

            properties[0].ChildProperties[1].Name.Should().Be("childStringProperty");
            properties[0].ChildProperties[1].Value.Should().Be("def");
            properties[0].ChildProperties[1].Raw.Should().NotBeEmpty();
            properties[0].ChildProperties[1].ChildProperties.Should().HaveCount(0);
        }
    }
}
