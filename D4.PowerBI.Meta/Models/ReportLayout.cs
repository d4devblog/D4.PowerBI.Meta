using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public class ReportLayout
    {
        public int Id { get; set; }

        public List<ReportPage> ReportPages { get; set; } = new();

        public List<ConfigurableProperty> Configuration { get; set; } = new();
    }
}
