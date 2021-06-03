using D4.PowerBI.Meta.Constants;
using System.Collections.Generic;

namespace D4.PowerBI.Meta.Models
{
    public static class VisualElementExtensions
    {
        private static readonly string[] _formattngNodePath = { 
            ReportLayoutDocument.SingleVisual, 
            ReportLayoutDocument.Objects };

        public static bool TryGetVisualFormatting(
            this VisualElement visualElement, 
            out List<ConfigurableProperty>? value)
        {
            visualElement.Configuration.TryGetProperty(
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
