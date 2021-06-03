using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public class ReportLayout
    {
        public int Id { get; set; }

        public List<ReportPage> ReportPages { get; set; } = new();

        public List<ConfigurableProperty> Configuration { get; set; } = new();
    }

    public class ReportConfiguration
    {
        public string Version { get; set; } = string.Empty;
    }

    public class ReportPage
    {
        public string Name { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public int Ordinal { get; set; } = 0;

        public List<VisualElement> VisualElements = new();

        public DisplayOption DisplayOption { get; set; } = DisplayOption.Unknown;

        public double Width { get; set; }

        public double Height { get; set; }

        public List<ConfigurableProperty> Configuration { get; set; } = new();
    }

    public enum DisplayOption
    {
        Unknown = 0,
        SixteenByNine = 1,
        FourByThree = 2,
        Letter = 3,
        Tooltip = 4,
        Custom = 5
    }

    public class VisualElement
    {
        public string Name { get; set; } = string.Empty;

        public string VisualType { get; set; } = string.Empty;

        public List<ConfigurableProperty> Configuration { get; set; } = new();

        public double X { get; set; }

        public double Y { get; set; }

        public double Z { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

    }

    
}
