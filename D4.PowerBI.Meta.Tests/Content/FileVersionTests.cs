using D4.PowerBI.Meta.Content;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace D4.PowerBI.Meta.Tests.Content
{
    public class FileVersionTests
    {
        [Theory]
        [InlineData("pbix/empty.pbix", "1.22")]
        [InlineData("pbix/empty.pbit", "1.22")]
        public async Task WHEN_pbi_file_is_read_THEN_expected_file_version_is_returned(
            string filename, string expectedVersion)
        {
            string? path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var fullPath = Path.Combine(path, filename);

            var sut = await PBIReader.OpenFileAsync(fullPath);
            var fileVersion = sut.ReadFileVersion();

            fileVersion.Should().Be(expectedVersion);
        }
    }
}
