using D4.PowerBI.Meta.Read;
using FluentAssertions;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace D4.PowerBI.Meta.Tests.Read
{
    [Collection("empty-pbix-tests")]
    public class FileVersionTests
    {
        private readonly string _testFilePath = string.Empty;
        public FileVersionTests()
        {
            _testFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        }

        [Theory]
        [InlineData("pbix/empty.pbix", "1.22")]
        [InlineData("pbix/empty.pbit", "1.22")]
        public async Task WHEN_pbi_file_is_read_THEN_expected_file_version_is_returned(
            string filename, string expectedVersion)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            var sut = await PBIReader.OpenFileAsync(fullPath);
            var fileVersion = sut.ReadFileVersion();

            fileVersion.Should().Be(expectedVersion);
        }
    }
}
