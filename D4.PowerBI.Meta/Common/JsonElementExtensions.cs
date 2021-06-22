using System.Linq;
using System.Text.Json;

namespace D4.PowerBI.Meta.Common
{
    internal static class JsonElementExtensions
    {
        internal static string? GetPropertyString(
            this JsonElement element, 
            string propertyName)
        {
            string? result = null;

            if(element.ValueKind == JsonValueKind.Object)
            {
                if (element.TryGetProperty(propertyName, out var elementValue))
                {
                    if (elementValue.ValueKind == JsonValueKind.String)
                    {
                        result = elementValue.GetString();
                    }
                }
            }

            return result;
        }

        internal static JsonElement? GetChild(this JsonElement element, string name)
        {
            return element.ValueKind != JsonValueKind.Null 
                && element.ValueKind != JsonValueKind.Undefined 
                && element.TryGetProperty(name, out var value)
                ? value 
                : (JsonElement?)null;
        }

        internal static JsonElement? GetOptionalChild(this JsonElement element, string name)
        {
            return element.ValueKind != JsonValueKind.Null
                && element.ValueKind != JsonValueKind.Undefined
                && element.TryGetProperty(name, out var value)
                ? value
                : element;
        }

        internal static JsonElement? GetChild(this JsonElement element, int index)
        {
            if (element.ValueKind == JsonValueKind.Null || element.ValueKind == JsonValueKind.Undefined)
            {
                return null;
            }

            var value = element.EnumerateArray().ElementAtOrDefault(index);
            return value.ValueKind != JsonValueKind.Undefined 
                ? value 
                : (JsonElement?)null;
        }
    }
}
