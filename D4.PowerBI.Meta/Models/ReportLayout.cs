using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public class ReportLayout
    {
        public int Id { get; set; }

        public List<ReportPage> ReportPages { get; set; } = new List<ReportPage>();

        public ReportConfiguration Config { get; set; } = new ReportConfiguration();
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
    }
}
