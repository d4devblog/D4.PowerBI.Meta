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
}
