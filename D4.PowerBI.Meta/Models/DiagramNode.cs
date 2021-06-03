namespace D4.PowerBI.Meta.Models
{
    public class DiagramNode
    {
        public Location Location { get; set; } = new();
        public string NodeIndex { get; set; } = string.Empty;
        public Size Size { get; set; } = new Size { Height = 150, Width = 200 };
        public int ZIndex { get; set; }
    }

}
