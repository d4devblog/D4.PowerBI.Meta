using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public class Diagram
    {
        public int Ordinal { get; set; }
        public Scrollposition ScrollPosition { get; set; } = new();
        public List<DiagramNode> Nodes { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public double ZoomValue { get; set; }
        public bool PinKeyFieldsToTop { get; set; }
        public bool ShowExtraHeaderInfo { get; set; }
        public bool HideKeyFieldsWhenCollapsed { get; set; }
    }

}
