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
                var filterType = GetFilterType(element);

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

        private static FilterType GetFilterType(JsonElement element)
        {
            var filterType = FilterType.Unknown;

            var typeProperty = element.GetPropertyString(ReportLayoutDocument.Type);
            if (typeProperty == null)
            {
                return filterType;
            }

            if (string.Compare(typeProperty, "Categorical", true) == 0)
            {
                filterType = FilterType.BasicCategorical;
            }
            else if (string.Compare(typeProperty, "TopN", true) == 0)
            {
                filterType = FilterType.TopN;
            }
            else if (string.Compare(typeProperty, "Advanced", true) == 0)
            {
                var whereArray = element.GetChild(ReportLayoutDocument.Filter)
                    ?.GetChild(ReportLayoutDocument.FilterWhere);

                if (whereArray != null && whereArray.Value.ValueKind == JsonValueKind.Array)
                {
                    var whereEnumerator = whereArray.Value.EnumerateArray();
                    whereEnumerator.MoveNext();

                    var comparisonKind = whereEnumerator.Current
                        .GetChild(ReportLayoutDocument.FilterCondition)
                        ?.GetChild(ReportLayoutDocument.FilterComparison)
                        ?.GetChild(ReportLayoutDocument.FilterComparisonKind);

                    var negatedComparisonKind = whereEnumerator.Current
                        .GetChild(ReportLayoutDocument.FilterCondition)
                        ?.GetChild(ReportLayoutDocument.FilterNot)
                        ?.GetChild(ReportLayoutDocument.FilterExpression)
                        ?.GetChild(ReportLayoutDocument.FilterComparison)
                        ?.GetChild(ReportLayoutDocument.FilterComparisonKind);

                    if (comparisonKind.HasValue &&
                        comparisonKind.Value.ValueKind == JsonValueKind.Number)
                    {
                        var kind = comparisonKind.Value.GetInt32();
                        filterType = kind switch
                        {
                            0 => GetDirectComparisonFllterType(whereEnumerator.Current),

                            1 => FilterType.IsGreaterThan,

                            2 => FilterType.IsGreaterThanOrEqualTo,

                            3 => FilterType.IsLessThan,

                            4 => FilterType.IsLessThanOrEqualTo,

                            _ => FilterType.Unknown
                        };
                    }

                    if (negatedComparisonKind.HasValue &&
                        negatedComparisonKind.Value.ValueKind == JsonValueKind.Number)
                    {
                        var kind = negatedComparisonKind.Value.GetInt32();
                        filterType = kind switch
                        {
                            0 => GetDirectComparisonFllterType(whereEnumerator.Current, true),

                            _ => FilterType.Unknown
                        };
                    }
                }
            }

            return filterType;
        }

        private static FilterType GetDirectComparisonFllterType(JsonElement whereElement, bool negateFilter = false)
        {
            var valueElement = whereElement
                .GetChild(ReportLayoutDocument.FilterCondition)
                ?.GetOptionalChild(ReportLayoutDocument.FilterNot)
                ?.GetOptionalChild(ReportLayoutDocument.FilterExpression)
                ?.GetChild(ReportLayoutDocument.FilterComparison)
                ?.GetChild(ReportLayoutDocument.FilterRight)
                ?.GetChild(ReportLayoutDocument.FilterLiteral)
                ?.GetChild(ReportLayoutDocument.FilterValue);

            var nullValue = valueElement.HasValue &&
                valueElement.Value.ValueKind == JsonValueKind.String &&
                valueElement.Value.GetString() == ReportLayoutDocument.FilterBlankValue;

            return negateFilter
                ? nullValue
                    ? FilterType.IsNotBlank
                    : FilterType.IsNot
                : nullValue
                    ? FilterType.IsBlank
                    : FilterType.Is;
        }
    }
}
