using D4.PowerBI.Meta.Constants;
using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public static class ReportPageExtensions
    {
        private static readonly string[] _formattngNodePath = {
            ReportLayoutDocument.Objects };

        public static bool TryGetPageFormatting(
            this ReportPage reportPage,
            out List<ConfigurableProperty>? value)
        {
            reportPage.Configuration.TryGetProperty(
                _formattngNodePath, out var formattingProperties);

            if (formattingProperties != null)
            {
                value = formattingProperties.ChildProperties;
                return true;
            }

            value = null;
            return false;
        }
    }
}
