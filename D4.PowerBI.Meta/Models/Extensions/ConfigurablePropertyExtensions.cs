using System.Collections.Generic;
using System.Linq;

namespace D4.PowerBI.Meta.Models
{
    public static class ConfigurablePropertyExtensions
    {
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
                value = currentProperty.Value;
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
