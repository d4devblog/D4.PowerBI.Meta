using System.Collections.Generic;
using System.Linq;

namespace D4.PowerBI.Meta.Models
{
    public class ConfigurableProperty
    {
        public string Name { get; set; } = string.Empty;
        public object? Value { get; set; } = null;
        public List<ConfigurableProperty> ChildProperties { get; set; } = new();
        public string Raw { get; set; } = string.Empty;

        //public object? TryGetValue(string[] nodes, out object? value)
        //{
        //    var properties = new List<ConfigurableProperty> { this };
        //    return TryGetValue(properties, nodes, out value);
        //}

        //public bool TryGetValue(List<ConfigurableProperty> properties, string[] nodes,
        //    out object? value)
        //{
        //    var currentProperty = new ConfigurableProperty
        //    {
        //        ChildProperties = properties
        //    };

        //    var p = nodes.GetEnumerator();
        //    while (p.MoveNext())
        //    {
        //        if (currentProperty != null && currentProperty.ChildProperties.Count > 0)
        //        {
        //            currentProperty = currentProperty.ChildProperties
        //                .FirstOrDefault(x => x.Name == p.Current.ToString());
        //        }
        //    }

        //    if (currentProperty != null && currentProperty.Name == nodes.Last())
        //    {
        //        value = currentProperty.Value;
        //        return true;
        //    }
        //    else
        //    {
        //        value = null;
        //        return false;
        //    }
        //}
    }
}
