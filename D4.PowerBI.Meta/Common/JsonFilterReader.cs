using D4.PowerBI.Meta.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace D4.PowerBI.Meta.Common
{
    public static class JsonFilterReader
    {
        public static List<FilterConfiguration> ReadFilterConfigurations(JsonElement element)
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
            return new FilterConfiguration();
        }
    }
}
