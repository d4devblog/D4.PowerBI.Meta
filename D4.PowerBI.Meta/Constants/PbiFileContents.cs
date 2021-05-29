using System.Collections.Generic;

namespace D4.PowerBI.Meta.Constants
{
    public static class PbiFileContents
    {
        public const string ContentTypes = "[Content_Types].xml";
        public const string Datamodel = "Datamodel";
        public const string DatamodelSchema = "DatamodelSchema";
        public const string DiagramLayout = "DiagramLayout";
        public const string Metadata = "Metadata";
        public const string ReportLayout = "Report/Layout";
        public const string SecurityBindings = "SecurityBindings";
        public const string Settings = "Settings";
        public const string Version = "Version";

        public static List<string> FileNames = new()
        {
            ContentTypes,
            DiagramLayout,
            Metadata,
            ReportLayout,
            SecurityBindings,
            Settings,
            Version
        };
    }
}