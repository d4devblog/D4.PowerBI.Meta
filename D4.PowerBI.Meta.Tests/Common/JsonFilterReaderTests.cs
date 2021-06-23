using D4.PowerBI.Meta.Common;
using D4.PowerBI.Meta.Constants;
using D4.PowerBI.Meta.Models;
using FluentAssertions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Xunit;

namespace D4.PowerBI.Meta.Tests.Common
{
    public class JsonFilterReaderTests
    {
        private readonly string _testFilePath = string.Empty;

        public JsonFilterReaderTests()
        {
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            _testFilePath = Path.Combine(root, "JSON Fragments", "Filters");
        }

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
        [InlineData("[{ \"type\": \"TopN\" } ]", 1)]
        [InlineData("[ { \"name\": \"Filter1\" }, { \"name\": \"Filter2\" } ]", 2)]
        [InlineData("[ { \"name\": \"Filter1\" }, { \"name\": \"Filter2\" }, { \"type\": \"TopN\" } ]", 3)]
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
            var filterNames = filters.Select(x => x.Name).Distinct().ToList();

            filterNames.Should().HaveCount(expectedCount);
            filterNames.ForEach(x => x.Should().BeOneOf("Filter1", "Filter2", string.Empty));
        }

        [Fact]
        public void WHEN_json_filter_contains_source_THEN_source_is_mapped()
        {
            var fullPath = Path.Combine(_testFilePath, "filter_with_basic_source.json");
            var jsonText = File.ReadAllText(fullPath);

            var filtersTestObject = new JsonFilterTestObject
            {
                Filters = jsonText
            };

            var document = JsonDocument.Parse(JsonSerializer.Serialize(filtersTestObject));
            var filterElement = document.RootElement.GetProperty(ReportLayoutDocument.Filters);

            var filters = JsonFilterReader.ReadFilterConfigurations(filterElement);
            filters.Should().HaveCount(1);

            filters[0].Name.Should().Be("Filter1");
            filters[0].SourceEntity.Should().Be("TableName");
            filters[0].SourceProperty.Should().Be("FieldName");
        }

        [Theory]
        [InlineData("unknown_filter.json", FilterType.Unknown)]
        [InlineData("basic_categorical_filter.json", FilterType.BasicCategorical)]
        [InlineData("advanced_greater_than_filter.json", FilterType.IsGreaterThan)]
        [InlineData("advanced_greater_than_or_equal_filter.json", FilterType.IsGreaterThanOrEqualTo)]
        [InlineData("advanced_less_than_filter.json", FilterType.IsLessThan)]
        [InlineData("advanced_less_than_or_equal_filter.json", FilterType.IsLessThanOrEqualTo)]
        [InlineData("advanced_is_filter.json", FilterType.Is)]
        [InlineData("advanced_is_not_filter.json", FilterType.IsNot)]
        [InlineData("advanced_is_blank_filter.json", FilterType.IsBlank)]
        [InlineData("advanced_is_not_blank_filter.json", FilterType.IsNotBlank)]
        [InlineData("top_n_filter.json", FilterType.TopN)]
        public void WHEN_json_filter_is_read_THEN_correct_type_is_returned(
            string jsonFile, FilterType expectedType)
        {
            var fullPath = Path.Combine(_testFilePath, jsonFile);
            var jsonText = File.ReadAllText(fullPath);

            var filtersTestObject = new JsonFilterTestObject
            {
                Filters = jsonText
            };

            var document = JsonDocument.Parse(JsonSerializer.Serialize(filtersTestObject));
            var filterElement = document.RootElement.GetProperty(ReportLayoutDocument.Filters);

            var filters = JsonFilterReader.ReadFilterConfigurations(filterElement);
            filters.Should().NotBeNull();

            filters.First().FilterType.Should().Be(expectedType);
            filters.First().SecondaryFilterType.Should().BeNull();
            filters.First().SecondaryFilterCondition.Should().Be(SecondaryFilterCondition.NotSet);
        }

        [Fact]
        public void WHEN_json_filter_contains_and_condition_THEN_correct_types_are_returned()
        {
            var fullPath = Path.Combine(_testFilePath, "advanced__and__filter.json");
            var jsonText = File.ReadAllText(fullPath);

            var filtersTestObject = new JsonFilterTestObject
            {
                Filters = jsonText
            };

            var document = JsonDocument.Parse(JsonSerializer.Serialize(filtersTestObject));
            var filterElement = document.RootElement.GetProperty(ReportLayoutDocument.Filters);

            var filters = JsonFilterReader.ReadFilterConfigurations(filterElement);
            filters.Should().NotBeNull();

            filters.First().FilterType.Should().Be(FilterType.IsNotBlank);
            filters.First().SecondaryFilterType.Should().Be(FilterType.IsGreaterThan);
            filters.First().SecondaryFilterCondition.Should().Be(SecondaryFilterCondition.And);
        }

        [Fact]
        public void WHEN_json_filter_contains_or_condition_THEN_correct_types_are_returned()
        {
            var fullPath = Path.Combine(_testFilePath, "advanced__or__filter.json");
            var jsonText = File.ReadAllText(fullPath);

            var filtersTestObject = new JsonFilterTestObject
            {
                Filters = jsonText
            };

            var document = JsonDocument.Parse(JsonSerializer.Serialize(filtersTestObject));
            var filterElement = document.RootElement.GetProperty(ReportLayoutDocument.Filters);

            var filters = JsonFilterReader.ReadFilterConfigurations(filterElement);
            filters.Should().NotBeNull();

            filters.First().FilterType.Should().Be(FilterType.IsLessThan);
            filters.First().SecondaryFilterType.Should().Be(FilterType.IsGreaterThan);
            filters.First().SecondaryFilterCondition.Should().Be(SecondaryFilterCondition.Or);
        }
    }
}
