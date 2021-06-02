using D4.PowerBI.Meta.Read;
using FluentAssertions;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace D4.PowerBI.Meta.Tests.Read
{
    public class DiagramLayoutTests
    {
        private const string AllTables = "All tables";
        private const string SelectedDiagram = "All tables";
        private const string LayoutOne = "Layout 1 - Table One Only";
        private const string LayoutTwo = "Layout 2 - Table Two Only";
        private const string TableOne = "TableOne";
        private const string TableTwo = "TableTwo";

        private readonly string _testFilePath = string.Empty;

        public DiagramLayoutTests()
        {
            _testFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        }

        [Theory]
        [InlineData("pbix/fixedDataWithDataLayout.pbix", 3, "1.1.0")]
        public async Task WHEN_pbi_file_is_read_THEN_diagram_layout_is_returned(
            string filename, int numberOfDiagrams, string version)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            var sut = await PBIReader.OpenFileAsync(fullPath);
            var diagramLayout = sut.ReadDiagramLayout();

            diagramLayout.Should().NotBeNull();
            diagramLayout?.Diagrams.Count.Should().Be(numberOfDiagrams);
            diagramLayout?.Version.Should().Be(version);
        }

        [Theory]
        [InlineData("pbix/fixedDataWithDataLayout.pbix")]
        public async Task WHEN_diagram_layout_returned_THEN_it_contains_expected_values(string filename)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            var sut = await PBIReader.OpenFileAsync(fullPath);
            var diagramLayout = sut.ReadDiagramLayout();

            diagramLayout.Should().NotBeNull();

            var diagramOne = diagramLayout?.Diagrams.First(x => x.Ordinal == 0);
            var diagramTwo = diagramLayout?.Diagrams.First(x => x.Ordinal == 1);
            var diagramThree = diagramLayout?.Diagrams.First(x => x.Ordinal == 2);

            diagramLayout?.DefaultDiagram.Should().Be(AllTables);
            diagramLayout?.SelectedDiagram.Should().Be(SelectedDiagram);

            diagramOne?.Name.Should().Be(AllTables);
            diagramOne?.Nodes.Should().HaveCount(2);
            diagramOne?.Nodes.First().NodeIndex.Should().Be(TableOne);
            diagramOne?.Nodes.First().Location.Should().NotBeNull();
            diagramOne?.Nodes.First().Size.Should().NotBeNull();
            diagramOne?.Nodes.Last().NodeIndex.Should().Be(TableTwo);
            diagramOne?.Nodes.Last().Location.Should().NotBeNull();
            diagramOne?.Nodes.Last().Size.Should().NotBeNull();

            diagramTwo?.Name.Should().Be(LayoutOne);
            diagramTwo?.Nodes.Should().HaveCount(1);
            diagramTwo?.Nodes.First().NodeIndex.Should().Be(TableOne);
            diagramTwo?.Nodes.First().Location.Should().NotBeNull();
            diagramTwo?.Nodes.First().Size.Should().NotBeNull();

            diagramThree?.Name.Should().Be(LayoutTwo);
            diagramThree?.Nodes.Should().HaveCount(1);
            diagramThree?.Nodes.First().NodeIndex.Should().Be(TableTwo);
            diagramThree?.Nodes.First().Location.Should().NotBeNull();
            diagramThree?.Nodes.First().Size.Should().NotBeNull();
        }
    }
}