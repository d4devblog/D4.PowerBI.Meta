using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace D4.PowerBI.Meta
{
    public static class PBIReader
    {
        public static PBIFile OpenFile(string path)
        {
            CheckFilePath(path);

            var fileData = File.ReadAllBytes(path);
            var fileStream = new MemoryStream(fileData);

            var pbiFile = new PBIFile(fileStream);
            pbiFile.Initialise();

            return pbiFile;
        }

        public static async Task<PBIFile> OpenFileAsync(string path, CancellationToken cancellationToken = default)
        {
            CheckFilePath(path);

            var fileData = await File.ReadAllBytesAsync(path, cancellationToken);
            var fileStream = new MemoryStream(fileData);

            var pbiFile = new PBIFile(fileStream);
            pbiFile.Initialise();

            return pbiFile;
        }

        public static PBIFile OpenFile(Stream fileStream)
        {
            CheckFileStream(fileStream);

            var pbiFile = new PBIFile(fileStream);
            pbiFile.Initialise();

            return pbiFile;
        }

        public static async Task<PBIFile> OpenFileAsync(Stream fileStream, CancellationToken cancellationToken = default)
        {
            CheckFileStream(fileStream);

            var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream, cancellationToken);
            memoryStream.Position = 0;

            var pbiFile = new PBIFile(memoryStream);
            pbiFile.Initialise();

            return pbiFile;
        }

        private static void CheckFilePath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("'path' must be supplied.");
            }

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"file: '{path}' was not found.");
            }
        }

        private static void CheckFileStream(Stream fileStream)
        {
            if (fileStream is null)
            {
                throw new ArgumentException("'fileStream' cannot be null.");
            }

            if (fileStream.Length == 0)
            {
                throw new ArgumentException("'fileStream' cannot be empty.");
            }

            if (!fileStream.CanRead)
            {
                throw new ArgumentException("'fileStream' cannot be read.");
            }
        }
    }
}