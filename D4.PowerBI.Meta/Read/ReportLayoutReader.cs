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
        public static ReportLayout ReadReportLayout(this PBIFile pbiFile)
        {
            var reportLayoutFile = pbiFile.ArchiveEntries
                .FirstOrDefault(x => x.FullName == PbiFileContents.ReportLayout);

            if (reportLayoutFile == null)
            {
                throw new ContentNotFoundException("Unavle to read Report Layout content.");
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
                Config = GetConfiguration(report.RootElement)
            };

            return reportLayout;
        }

        private static int GetReportId(JsonElement rootElement)
        {
            return rootElement.GetProperty("id").GetInt32();
        }

        private static ReportConfiguration GetConfiguration(JsonElement rootElement)
        {
            var config = rootElement.GetProperty(ReportLayoutDocument.ReportConfigNode);
            var configJson = config.GetString();

            return new ReportConfiguration();
        }

        private static List<ReportPage> GetPages(JsonElement rootElement)
        {
            var reportPages = rootElement.GetProperty(ReportLayoutDocument.PageArrayNode);

            var e = reportPages.EnumerateArray();
            var pages = e.Select(x =>
            {
                return new ReportPage();
            }).ToList();

            return pages;
        }
    }
}
