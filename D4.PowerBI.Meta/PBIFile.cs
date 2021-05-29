using D4.PowerBI.Meta.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace D4.PowerBI.Meta
{
    public class PBIFile : IDisposable
    {
        private readonly Stream _fileStream;
        private ZipArchive? _archive = null;
        private IList<ZipArchiveEntry> _archiveEntries = new List<ZipArchiveEntry>();

        public PBIFile(Stream fileStream)
        {
            _fileStream = fileStream;
        }

        public bool IsValidPbiFile
        {
            get
            {
                var matchFileCount = _archiveEntries.Count(x =>
                    PbiFileContents.FileNames.Contains(x.FullName));

                return matchFileCount == PbiFileContents.FileNames.Count;
            }
        }

        public bool CanRead => _fileStream.CanRead;
        public long FileLength => _fileStream.Length;

        internal IList<ZipArchiveEntry> ArchiveEntries => _archiveEntries;

        public void Initialise()
        {
            _archive = new ZipArchive(_fileStream, ZipArchiveMode.Read, true);
            _archiveEntries = _archive.Entries;
        }

        public void Dispose()
        {
            if (_archive != null)
            {
                _archive.Dispose();
            }
        }
    }
}
