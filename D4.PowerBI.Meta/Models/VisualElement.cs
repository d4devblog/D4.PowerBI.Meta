using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public class VisualElement
    {
        public string Name { get; set; } = string.Empty;

        public string VisualType { get; set; } = string.Empty;

        public List<ConfigurableProperty> Configuration { get; set; } = new();

        public Location Location { get; set; } = new();

        public Size Size { get; set; } = new();
    }
}
