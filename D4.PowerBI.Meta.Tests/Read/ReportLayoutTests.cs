using D4.PowerBI.Meta.Models;
using D4.PowerBI.Meta.Read;
using FluentAssertions;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace D4.PowerBI.Meta.Tests.Read
{
    public class ReportLayoutTests
    {
        [Theory]
        [InlineData("pbix/reportPagesWithShapes.pbix")]
        public async Task WHEN_pbi_file_is_read_THEN_report_layout_is_returned(
            string filename)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fullPath = Path.Combine(path, filename);

            var sut = await PBIReader.OpenFileAsync(fullPath);
            var reportLayout = sut.ReadReportLayout();

            reportLayout.Should().NotBeNull();
            reportLayout.Configuration.Count.Should().BeGreaterThan(0);
            reportLayout.ReportPages.Count.Should().Be(2);

            reportLayout.ReportPages[0].Name.Should().Be("ReportSection");
            reportLayout.ReportPages[0].DisplayName.Should().Be("Page With Shape");
            reportLayout.ReportPages[0].Ordinal.Should().Be(0);

            reportLayout.ReportPages[1].Name.Should().Be("ReportSection8ce75128eea8b556229d");
            reportLayout.ReportPages[1].DisplayName.Should().Be("Page With Text");
            reportLayout.ReportPages[1].Ordinal.Should().Be(1);

            reportLayout.ReportPages.ForEach(x =>
            {
                x.Width.Should().Be(1280);
                x.Height.Should().Be(720);
                x.DisplayOption.Should().Be(DisplayOption.SixteenByNine);
                x.VisualElements.Should().NotBeNull();
                x.Configuration.Count.Should().BeGreaterThan(0);
            });
        }
    }
}
