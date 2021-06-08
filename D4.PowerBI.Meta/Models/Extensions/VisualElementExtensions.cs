using D4.PowerBI.Meta.Constants;
using System.Collections.Generic;
using System.Linq;

namespace D4.PowerBI.Meta.Models
{
    public static class VisualElementExtensions
    {
        private static readonly string[] _formattngObjectsNodePath = { 
            ReportLayoutDocument.SingleVisual, 
            ReportLayoutDocument.Objects };

        private static readonly string[] _formattngVcObjectsNodePath = {
            ReportLayoutDocument.SingleVisual,
            ReportLayoutDocument.VcObjects };

        public static bool TryGetVisualFormatting(
            this VisualElement visualElement, 
            out List<ConfigurableProperty>? value)
        {
            visualElement.Configuration.TryGetProperty(
                _formattngObjectsNodePath, out var formattingObjectProperties);

            visualElement.Configuration.TryGetProperty(
                _formattngVcObjectsNodePath, out var formattingVcObjectProperties);

            var combinedList = new List<ConfigurableProperty>();

            if (formattingObjectProperties != null)
            {
                combinedList.AddRange(formattingObjectProperties.ChildProperties);
            }

            if (formattingVcObjectProperties != null)
            {
                combinedList.AddRange(formattingVcObjectProperties.ChildProperties);
            }

            if (combinedList.Any())
            {
                value = combinedList;
                return true;
            }

            value = null;
            return false;
        }
    }
}
