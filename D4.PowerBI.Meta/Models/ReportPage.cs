using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public class ReportPage
    {
        public string Name { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public int Ordinal { get; set; } = 0;

        public List<VisualElement> VisualElements = new();

        public ReportPageDisplayOption DisplayOption { get; set; } = ReportPageDisplayOption.Unknown;

        public Size Size { get; set; } = new();

        public List<ConfigurableProperty> Configuration { get; set; } = new();
    }
}
