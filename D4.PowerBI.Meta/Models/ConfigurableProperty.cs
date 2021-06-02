using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public class ConfigurableProperty
    {
        public string Name { get; set; } = string.Empty;
        public object? Value { get; set; } = null;
        public List<ConfigurableProperty> ChildProperties { get; set; } = new();
        public string Raw { get; set; } = string.Empty;
    }
}
