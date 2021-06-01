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
            reportLayout.Config.Should().NotBeNull();
            reportLayout.ReportPages.Count.Should().Be(2);
        }
    }
}
