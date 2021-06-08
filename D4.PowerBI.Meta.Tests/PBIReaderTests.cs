using D4.PowerBI.Meta.Tests.Utility;
using FluentAssertions;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace D4.PowerBI.Meta.Tests
{
    [Collection("empty-pbix-tests")]
    public class PBIReaderTests
    {
        private readonly string _testFilePath = string.Empty;

        public PBIReaderTests()
        {
            _testFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
        }

#pragma warning disable CS8604 // Possible null reference argument is intentional.
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void WHEN_no_path_supplied_THEN_argument_exception_thrown(string? path)
        {
            Func<PBIFile> sut = () => PBIReader.OpenFile(path);
            sut.Should().Throw<ArgumentException>();

            Func<Task<PBIFile>> sutAsync = async () => await PBIReader.OpenFileAsync(path);
            sutAsync.Should().Throw<ArgumentException>();
        }
#pragma warning restore CS8604

        [Theory]
        [InlineData("pbix/does-not-exist.pbix")]
        public void WHEN_path_does_not_resolve_to_a_file_THEN_file_not_found_exception_thrown(string filename)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            Func<PBIFile> sut = () => PBIReader.OpenFile(fullPath);
            sut.Should().Throw<FileNotFoundException>()
                .WithMessage($"file: '{fullPath}' was not found.");

            Func<Task<PBIFile>> sutAsync = async () => await PBIReader.OpenFileAsync(fullPath);
            sutAsync.Should().Throw<FileNotFoundException>()
                .WithMessage($"file: '{fullPath}' was not found.");
        }

        [Theory]
        [InlineData("pbix/_empty.pbix")]
        [InlineData("pbix/_empty.pbit")]
        public void WHEN_path_resolves_to_a_pbix_or_pbit_THEN_the_file_is_read_and_pbi_file_object_returned(string filename)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            var sut = PBIReader.OpenFile(fullPath);
            sut.Should().NotBeNull();
            sut.CanRead.Should().BeTrue();
            sut.FileLength.Should().BeGreaterThan(0);
            sut.IsValidPbiFile.Should().BeTrue();
        }

        [Theory]
        [InlineData("pbix/_empty.pbix")]
        [InlineData("pbix/_empty.pbit")]
        public async Task WHEN_path_resolves_to_a_pbix_or_pbit_THEN_the_file_is_read_async_nd_pbi_file_object_returned(string filename)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            var sut = await PBIReader.OpenFileAsync(fullPath);
            sut.Should().NotBeNull();
            sut.CanRead.Should().BeTrue();
            sut.FileLength.Should().BeGreaterThan(0);
            sut.IsValidPbiFile.Should().BeTrue();
        }

#pragma warning disable CS8604 // Possible null reference argument is intentional.
        [Fact]
        public void WHEN_stream_is_null_THEN_an_argument_exception_is_thrown()
        {
            Stream? stream = null;

            Func<PBIFile> sut = () => PBIReader.OpenFile(stream);

            sut.Should().Throw<ArgumentException>()
                .WithMessage("'fileStream' cannot be null.");

            Func<Task<PBIFile>> sutAsync = async () => await PBIReader.OpenFileAsync(stream);
            sutAsync.Should().Throw<ArgumentException>()
                .WithMessage($"'fileStream' cannot be null.");
        }
#pragma warning restore CS8604

        [Fact]
        public void WHEN_stream_is_empty_THEN_an_argument_exception_is_thrown()
        {
            var stream = new MemoryStream();

            Func<PBIFile> sut = () => PBIReader.OpenFile(stream);
            sut.Should().Throw<ArgumentException>()
                .WithMessage("'fileStream' cannot be empty.");

            Func<Task<PBIFile>> sutAsync = async () => await PBIReader.OpenFileAsync(stream);
            sutAsync.Should().Throw<ArgumentException>()
                .WithMessage($"'fileStream' cannot be empty.");
        }

        [Fact]
        public void WHEN_stream_cannot_be_read_THEN_an_argument_exception_is_thrown()
        {
            var stream = new WriteOnlyStream();

            Func<PBIFile> sut = () => PBIReader.OpenFile(stream);
            sut.Should().Throw<ArgumentException>()
                .WithMessage("'fileStream' cannot be read.");

            Func<Task<PBIFile>> sutAsync = async () => await PBIReader.OpenFileAsync(stream);
            sutAsync.Should().Throw<ArgumentException>()
                .WithMessage($"'fileStream' cannot be read.");
        }

        [Theory]
        [InlineData("pbix/_empty.pbix")]
        [InlineData("pbix/_empty.pbit")]
        public void WHEN_stream_contains_pbix_or_pbit_data_THEN_the_file_is_read_and_pbi_file_object_returned(string filename)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            using var fileStream = new FileStream(fullPath, FileMode.Open);
            var length = fileStream.Length;

            var sut = PBIReader.OpenFile(fileStream);
            sut.Should().NotBeNull();
            sut.CanRead.Should().BeTrue();
            sut.FileLength.Should().Be(length);
            sut.IsValidPbiFile.Should().BeTrue();
        }

        [Theory]
        [InlineData("pbix/_empty.pbix")]
        [InlineData("pbix/_empty.pbit")]
        public async Task WHEN_stream_contains_pbix_or_pbit_data_THEN_the_file_is_read_async_and_pbi_file_object_returned(string filename)
        {
            var fullPath = Path.Combine(_testFilePath, filename);

            using var asyncFileStream = new FileStream(fullPath, FileMode.Open);

            var sutAsync = await PBIReader.OpenFileAsync(asyncFileStream);
            sutAsync.Should().NotBeNull();
            sutAsync.CanRead.Should().BeTrue();
            sutAsync.FileLength.Should().Be(asyncFileStream.Length);
            sutAsync.IsValidPbiFile.Should().BeTrue();
        }
    }
}
