using System.Collections.Generic;
using System.Linq;

namespace D4.PowerBI.Meta.Models
{
    public static class ConfigurablePropertyExtensions
    {
        public static bool TryGetProperty(this ConfigurableProperty property,
            string[] nodes, out ConfigurableProperty? value)
        {
            var properties = new List<ConfigurableProperty> { property };
            return TryGetProperty(properties, nodes, out value);
        }

        public static bool TryGetProperty(
            this List<ConfigurableProperty> properties,
            string[] nodes, out ConfigurableProperty? value)
        {
            var matchingProperty = SearchPropertyNodes(properties, nodes);
            if (matchingProperty != null)
            {
                value = matchingProperty;
                return true;
            }

            value = null;
            return false;
        }

        public static bool TryGetValue(this ConfigurableProperty property,
            string[] nodes, out object? value)
        {
            var properties = new List<ConfigurableProperty> { property };
            return TryGetValue(properties, nodes, out value);
        }

        public static bool TryGetValue(
            this List<ConfigurableProperty> properties,
            string[] nodes, out object? value)
        {
            var matchingProperty = SearchPropertyNodes(properties, nodes);
            if (matchingProperty != null)
            {
                value = matchingProperty.Value;
                return true;
            }

            value = null;
            return false;
        }

        private static ConfigurableProperty? SearchPropertyNodes(List<ConfigurableProperty> properties, string[] nodes)
        {
            var currentProperty = new ConfigurableProperty
            {
                ChildProperties = properties
            };

            var e = nodes.GetEnumerator();
            while (e.MoveNext())
            {
                if (currentProperty != null && currentProperty.ChildProperties.Count > 0)
                {
                    currentProperty = currentProperty.ChildProperties
                        .FirstOrDefault(x => x.Name == e.Current.ToString());
                }
            }

            if (currentProperty != null && currentProperty.Name == nodes.Last())
            {
                return currentProperty;
            }

            return null;
        }
    }
}
