using D4.PowerBI.Meta.Common;
using D4.PowerBI.Meta.Constants;
using FluentAssertions;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace D4.PowerBI.Meta.Tests.Common
{
    public class JsonFilterReaderTests
    {
        public class JsonFilterTestObject
        {
            [JsonPropertyName("filters")]
            public string? Filters { get; set; }
        }

        [Theory]
        [InlineData("{}")]
        [InlineData("[]")]
        public void WHEN_json_filter_is_effectively_empty_THEN_empty_list_returned(
            string json)
        {
            var document = JsonDocument.Parse(json);

            var filters = JsonFilterReader.ReadFilterConfigurations(document.RootElement);
            filters.Should().NotBeNull();
            filters.Should().HaveCount(0);
        }

        [Theory]
        [InlineData("{ \"filter\":{ \"type\": \"TopN\" } }", 1)]
        [InlineData("[ { \"name\": \"Filter1\" }, { \"name\": \"Filter2\" } ]", 2)]
        [InlineData("[ { \"name\": \"Filter1\" }, { \"name\": \"Filter2\" }, { \"filter\":{ \"type\": \"TopN\" } } ]", 3)]
        public void WHEN_json_filter_contains_multiple_items_THEN_full_list_returned(
            string json, int expectedCount)
        {
            var filtersTestObject = new JsonFilterTestObject
            {
                Filters = json
            };

            var document = JsonDocument.Parse(JsonSerializer.Serialize(filtersTestObject));
            var filterElement = document.RootElement.GetProperty(ReportLayoutDocument.Filters);

            var filters = JsonFilterReader.ReadFilterConfigurations(filterElement);
            filters.Should().NotBeNull();
            filters.Should().HaveCount(expectedCount);

            filters.TrueForAll(x => x.Name != null);
            var filterNames = filters.Select(x => x.Name).ToList();
            filterNames.ForEach(x => x.Should().BeOneOf("Filter1", "Filter2", string.Empty));
        }
    }
}
