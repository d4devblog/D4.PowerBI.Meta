using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace D4.PowerBI.Meta
{
    public class PBIFile
    {
        private readonly bool _canRead = false;
        private readonly long _fileLength = 0;
        private readonly Stream _fileStream;
        private IList<ZipArchiveEntry> _archiveEntries = new List<ZipArchiveEntry>();
        private readonly List<string> _archiveFilenames = new() 
        {
            "[Content_Types].xml",
            "DiagramLayout",
            "Metadata",
            "Report/Layout",
            "SecurityBindings", 
            "Settings",
            "Version"
        };

        public PBIFile(Stream fileStream)
        {
            _fileStream = fileStream;
            _canRead = fileStream.Length > 0;
            _fileLength = fileStream.Length;
        }

        public bool IsValidPbiFile
        {
            get
            {
                var matchFileCount = _archiveEntries.Count(x => 
                    _archiveFilenames.Contains(x.FullName));

                return matchFileCount == _archiveFilenames.Count;
            }
        }

        public bool CanRead => _canRead;
        public long FileLength => _fileLength;

        public void Initialise()
        {
            using var archive = new ZipArchive(_fileStream);
            _archiveEntries = archive.Entries;
        }
    }
}
