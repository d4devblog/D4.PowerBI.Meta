using D4.PowerBI.Meta.Constants;
using D4.PowerBI.Meta.Models;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json;

[assembly: InternalsVisibleTo("D4.PowerBI.Meta.Tests")]
namespace D4.PowerBI.Meta.Common
{
    internal static class JsonFilterReader
    {
        internal static List<FilterConfiguration> ReadFilterConfigurations(JsonElement element)
        {
            var filters = new List<FilterConfiguration>();

            if (element.ValueKind == JsonValueKind.String)
            {
                var filterString = element.GetString();
                if (!string.IsNullOrWhiteSpace(filterString))
                {
                    var filterDocument = JsonDocument.Parse(filterString);

                    if (filterDocument.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        filters.Add(BuildFilterConfiguration(filterDocument.RootElement));
                    }
                    else if (filterDocument.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        var filterElement = filterDocument.RootElement.EnumerateArray();
                        while (filterElement.MoveNext())
                        {
                            filters.Add(BuildFilterConfiguration(filterElement.Current));
                        }
                    }
                }
            }

            return filters;
        }

        private static FilterConfiguration BuildFilterConfiguration(JsonElement element)
        {
            if (element.ValueKind == JsonValueKind.Object)
            {
                var filterName = element.GetPropertyString(ReportLayoutDocument.Name)?? string.Empty;
                var filterEntity = GetFilterSourceEntity(element);
                var filterProperty = GetFilterSourceProperty(element);
                var filterType = GetFiltertype(element);

                return new FilterConfiguration
                {
                    Name = filterName,
                    SourceEntity = filterEntity,
                    SourceProperty = filterProperty,
                    FilterType = filterType,
                };
            }

            return new FilterConfiguration();
        }

        private static string GetFilterSourceEntity(JsonElement element)
        {
            var entity = element.GetChild(ReportLayoutDocument.Expression)
                            ?.GetChild(ReportLayoutDocument.FilterColumn)
                            ?.GetChild(ReportLayoutDocument.FilterExpression)
                            ?.GetChild(ReportLayoutDocument.FilterSourceRef)
                            ?.GetChild(ReportLayoutDocument.FilterEntity);

            return entity.HasValue
                ? entity.Value.GetString()?? string.Empty
                : string.Empty;
        }

        private static string GetFilterSourceProperty(JsonElement element)
        {
            var property = element.GetChild(ReportLayoutDocument.Expression)
                            ?.GetChild(ReportLayoutDocument.FilterColumn)
                            ?.GetChild(ReportLayoutDocument.FilterProperty);

            return property.HasValue
                ? property.Value.GetString() ?? string.Empty
                : string.Empty;
        }

        private static FilterType GetFiltertype(JsonElement element)
        {
            var filterType = FilterType.Unknown;

            //var f1 = element.GetChild("filter");
            //var typeProperty = element.GetChild(ReportLayoutDocument.Type);

            var typeProperty = element.GetPropertyString(ReportLayoutDocument.Type);

            if (typeProperty != null && typeProperty == "Categorical")
            {
                filterType = FilterType.BasicCategorical;
            }

            return filterType;
        }
    }
}
