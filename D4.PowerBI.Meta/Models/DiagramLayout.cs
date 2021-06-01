using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public class DiagramLayout
    {
        public string Version { get; set; } = string.Empty;
        public List<Diagram> Diagrams { get; set; } = new();
        public string SelectedDiagram { get; set; } = string.Empty;
        public string DefaultDiagram { get; set; } = string.Empty;
    }

    public class Diagram
    {
        public int Ordinal { get; set; }
        public Scrollposition ScrollPosition { get; set; } = new Scrollposition { X = 0, Y = 0 };
        public List<Node> Nodes { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public float ZoomValue { get; set; }
        public bool PinKeyFieldsToTop { get; set; }
        public bool ShowExtraHeaderInfo { get; set; }
        public bool HideKeyFieldsWhenCollapsed { get; set; }
    }

    public class Scrollposition
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class Node
    {
        public Location Location { get; set; } = new Location { X = 0, Y = 0 };
        public string NodeIndex { get; set; } = string.Empty;
        public Size Size { get; set; } = new Size { Height = 150, Width = 200 };
        public int ZIndex { get; set; }
    }

    public class Location
    {
        public float X { get; set; }
        public float Y { get; set; }
    }

    public class Size
    {
        public int Height { get; set; }
        public int Width { get; set; }
    }

}
