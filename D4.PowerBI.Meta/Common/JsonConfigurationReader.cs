﻿using D4.PowerBI.Meta.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace D4.PowerBI.Meta.Common
{
    public static class JsonConfigurationReader
    {
        public static List<ConfigurableProperty> ReadPropertyConfigurationNode(
            JsonElement element)
        {
            var properties = new List<ConfigurableProperty>();

            if (element.ValueKind == JsonValueKind.String)
            {
                var configString = element.GetString();
                if (!string.IsNullOrWhiteSpace(configString))
                {
                    var configDocument = JsonDocument.Parse(configString);
                    properties = AddChildProperties(properties, configDocument?.RootElement);
                }
            }
            
            return properties;
        }

        private static List<ConfigurableProperty> AddChildProperties(
            List<ConfigurableProperty> properties, 
            JsonElement? element)
        {
            if (!element.HasValue)
            {
                return properties;
            }

            if (element.Value.ValueKind == JsonValueKind.Object)
            {
                var nextElement = element.Value.EnumerateObject();
                while (nextElement.MoveNext())
                {
                    var newProperty = new ConfigurableProperty
                    {
                        Name = nextElement.Current.Name,
                        Raw = nextElement.Current.Value.GetRawText()
                    };

                    switch (nextElement.Current.Value.ValueKind)
                    {
                        case JsonValueKind.Object:
                            newProperty.ChildProperties = AddChildProperties(
                                new List<ConfigurableProperty>(),
                                nextElement.Current.Value);
                            break;
                        case JsonValueKind.String:
                            newProperty.Value = nextElement.Current.Value.GetString();
                            break;
                        case JsonValueKind.Number:
                            newProperty.Value = nextElement.Current.Value.GetDecimal();
                            break;
                        case JsonValueKind.True:
                            newProperty.Value = true;
                            break;
                        case JsonValueKind.False:
                            newProperty.Value = false;
                            break;
                        case JsonValueKind.Array:
                            var array = new object[nextElement.Current.Value.GetArrayLength()];
                            var arrayItem = nextElement.Current.Value.EnumerateArray();

                            int i = 0;
                            while (arrayItem.MoveNext())
                            {
                                switch (arrayItem.Current.ValueKind)
                                {
                                    case JsonValueKind.String:
                                        array[i] = arrayItem.Current.GetString() ?? "";
                                        break;
                                    case JsonValueKind.Number:
                                        array[i] = arrayItem.Current.GetDecimal();
                                        break;
                                    default: break;
                                }

                                i++;
                            }
                            newProperty.Value = array;
                            break;
                        default:
                            break;
                    }

                    properties.Add(newProperty);
                }
            }
            else if (element.Value.ValueKind == JsonValueKind.Array)
            {
                var nextPropery = element.Value.EnumerateArray();
                while (nextPropery.MoveNext())
                {
                    return AddChildProperties(properties, nextPropery.Current);
                }
            }

            return properties;
        }
    }
}
