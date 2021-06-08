using D4.PowerBI.Meta.Models;
using D4.PowerBI.Meta.Read;
using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace D4.PowerBI.Meta.Tests.Read
{
    public class ReportLayoutTests
    {
        private readonly string _testFilePath = string.Empty;

        public ReportLayoutTests()
        {
            _testFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        }

        [Theory]
        [InlineData("pbix/reportPagesWithShapes.pbix")]
        public async Task WHEN_pbi_file_is_read_THEN_report_layout_is_returned(
            string filename)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            var sut = await PBIReader.OpenFileAsync(fullPath);
            var reportLayout = sut.ReadReportLayout();

            reportLayout.Should().NotBeNull();
            reportLayout.Configuration.Count.Should().BeGreaterThan(0);
            reportLayout.ReportPages.Should().HaveCount(2);

            reportLayout.ReportPages[0].Name.Should().Be("ReportSection");
            reportLayout.ReportPages[0].DisplayName.Should().Be("Page With Shape");
            reportLayout.ReportPages[0].Ordinal.Should().Be(0);
            reportLayout.ReportPages[0]
                .TryGetPageFormatting(out var pageOneFormatting)
                .Should().BeTrue();

            reportLayout.ReportPages[1].Name.Should().Be("ReportSection8ce75128eea8b556229d");
            reportLayout.ReportPages[1].DisplayName.Should().Be("Page With Text");
            reportLayout.ReportPages[1].Ordinal.Should().Be(1);
            reportLayout.ReportPages[1]
                .TryGetPageFormatting(out var pageTwoFormatting)
                .Should().BeTrue();

            pageOneFormatting.Should().HaveCountGreaterThan(0);
            pageTwoFormatting.Should().HaveCountGreaterThan(0);

            reportLayout.ReportPages.ForEach(x =>
            {
                x.Size.Width.Should().Be(1280);
                x.Size.Height.Should().Be(720);
                x.DisplayOption.Should().Be(ReportPageDisplayOption.SixteenByNine);
                x.VisualElements.Should().HaveCount(1);
                x.Configuration.Should().HaveCountGreaterThan(0);

                x.VisualElements[0].Name.Should().BeOneOf("ead243d7cba56d441a8a", "acc05e753689024961e8");
                x.VisualElements[0].VisualType.Should().BeOneOf("shape", "textbox");
                x.VisualElements[0].Configuration.Should().HaveCountGreaterThan(0);
                x.VisualElements[0].Size.Width.Should().BeGreaterThan(0);
                x.VisualElements[0].Size.Height.Should().BeGreaterThan(0);
                x.VisualElements[0].Location.X.Should().BeGreaterThan(0);
                x.VisualElements[0].Location.Y.Should().BeGreaterThan(0);
                x.VisualElements[0].Location.Z.Should().BeGreaterOrEqualTo(0);

                x.VisualElements[0].TryGetVisualFormatting(out var formattingProperties)
                    .Should().BeTrue();

                formattingProperties.Should().HaveCountGreaterThan(0);
            });
        }

        [Theory]
        [InlineData("pbix/basicVisualsWithSinpleFormatting.pbix")]
        public async Task WHEN_file_supplied_THEN_all_visual_formatting_can_be_read(
            string filename)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            var sut = await PBIReader.OpenFileAsync(fullPath);
            var reportLayout = sut.ReadReportLayout();

            reportLayout.Should().NotBeNull();

            reportLayout.ReportPages.Should().HaveCount(1);
            var visuals = reportLayout.ReportPages[0].VisualElements;

            visuals.Should().HaveCount(5);
            var textBoxes = visuals.FindAll(x => x.VisualType == "textbox");
            var cards = visuals.FindAll(x => x.VisualType == "card");
            var barChart = visuals.Find(x => x.VisualType == "clusteredBarChart");

            textBoxes.Should().HaveCount(2);
            cards.Should().HaveCount(2);
            barChart.Should().NotBeNull();

            var barChartFormatting = new List<ConfigurableProperty>();
            var foundFormatting = barChart?.TryGetVisualFormatting(out barChartFormatting);

            foundFormatting.Should().BeTrue();
            barChartFormatting.Should().NotBeNull();
            barChartFormatting?.Count.Should().BeGreaterThan(0);

            if (barChartFormatting != null && barChartFormatting.TryGetProperty(new string[3] 
                { "labels", "properties", "backgroundColor" }
                , out var labelBackground))
            {
                labelBackground.Should().NotBeNull();
                labelBackground?.ChildProperties.Should().HaveCountGreaterThan(0);
                labelBackground?.GetPropertyType()
                                .Should()
                                .Be(ConfigurablePropertyType.solidColor);
            }

            if (barChartFormatting != null && barChartFormatting.TryGetProperty(new string[3]
                { "background", "properties", "show" }
                , out var backgroundShow))
            {
                backgroundShow.Should().NotBeNull();
                backgroundShow?.ChildProperties.Should().HaveCountGreaterThan(0);
            }
        }
    }
}
