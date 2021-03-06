using D4.PowerBI.Meta.Common;
using D4.PowerBI.Meta.Constants;
using D4.PowerBI.Meta.Exceptions;
using D4.PowerBI.Meta.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace D4.PowerBI.Meta.Read
{
    public static class ReportLayoutReader
    {
        private static readonly string[] _visualNameNodePath = { ReportLayoutDocument.Name };
        private static readonly string[] _visualTypeNodePath = { ReportLayoutDocument.SingleVisual, ReportLayoutDocument.VisualType };

        public static ReportLayout ReadReportLayout(this PBIFile pbiFile)
        {
            var reportLayoutFile = pbiFile.ArchiveEntries
                .FirstOrDefault(x => x.FullName == PbiFileContents.ReportLayout);

            if (reportLayoutFile == null)
            {
                throw new ContentNotFoundException("Unable to read Report Layout content.");
            }

            var reader = new StreamReader(reportLayoutFile.Open(), Encoding.Unicode);
            var reportLayoutFileContent = reader.ReadToEnd();

            if (reportLayoutFileContent == null)
            {
                throw new ContentEmptyException("Report Layout is empty");
            }

            var options = new JsonDocumentOptions { AllowTrailingCommas = true };
            var report = JsonDocument.Parse(reportLayoutFileContent, options);

            var reportLayout = new ReportLayout
            {
                Id = GetReportId(report.RootElement),
                ReportPages = GetPages(report.RootElement),
                Configuration = GetConfiguration(report.RootElement)
            };

            return reportLayout;
        }

        private static int GetReportId(JsonElement rootElement)
        {
            return rootElement.GetProperty("id").GetInt32();
        }

        private static List<ConfigurableProperty> GetConfiguration(JsonElement element)
        {
            var config = element.GetProperty(ReportLayoutDocument.Configuration);
            return JsonConfigurationReader.ReadPropertyConfigurationNode(config);
        }

        private static List<ReportPage> GetPages(JsonElement rootElement)
        {
            var reportPages = rootElement.GetProperty(ReportLayoutDocument.PageArrayNode);

            var e = reportPages.EnumerateArray();
            return e.Select(x =>
            {
                return new ReportPage
                {
                    Name = x.GetProperty(ReportLayoutDocument.Name).GetString() ?? string.Empty,
                    DisplayName = x.GetProperty(ReportLayoutDocument.DisplayName).GetString() ?? string.Empty,
                    Ordinal = x.GetProperty(ReportLayoutDocument.Ordinal).GetInt32(),
                    Size = new Size
                    {
                        Width = x.GetProperty(ReportLayoutDocument.Width).GetDouble(),
                        Height = x.GetProperty(ReportLayoutDocument.Height).GetDouble(),
                    },
                    DisplayOption = (ReportPageDisplayOption)(x.GetProperty(ReportLayoutDocument.DisplayOption).GetInt32()),
                    Configuration = GetConfiguration(x),
                    VisualElements = GetVisuals(x)
                };
            }).ToList();
        }

        private static List<VisualElement> GetVisuals(JsonElement element)
        {
            var visualsElements = element.GetProperty(ReportLayoutDocument.VisualContainers);

            var e = visualsElements.EnumerateArray();
            return e.Select(x =>
            {
                var config = GetConfiguration(x);
                config.TryGetValue(_visualNameNodePath, out var name);
                config.TryGetValue(_visualTypeNodePath, out var visualType);

                return new VisualElement
                {
                    Name = name?.ToString() ?? string.Empty,
                    VisualType = visualType?.ToString() ?? string.Empty,
                    Configuration = config,
                    Size = new Size
                    {
                        Width = x.GetProperty(ReportLayoutDocument.Width).GetDouble(),
                        Height = x.GetProperty(ReportLayoutDocument.Height).GetDouble()
                    },
                    Location = new Location
                    {
                        X = x.GetProperty(ReportLayoutDocument.PosX).GetDouble(),
                        Y = x.GetProperty(ReportLayoutDocument.PosY).GetDouble(),
                        Z = x.GetProperty(ReportLayoutDocument.PosZ).GetDouble()
                    }
                };
            }).ToList();
        }
    }
}
