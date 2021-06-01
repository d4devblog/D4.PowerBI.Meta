using D4.PowerBI.Meta.Constants;
using System.IO;
using System.Linq;
using System.Text;

namespace D4.PowerBI.Meta.Read
{
    public static class FileVersionReader
    {
        public static string ReadFileVersion(this PBIFile pbiFile)
        {
            var fileVersion = string.Empty;

            var versionFile = pbiFile.ArchiveEntries
                .FirstOrDefault(x => x.FullName == PbiFileContents.Version);

            if (versionFile != null)
            {
                var reader = new StreamReader(versionFile.Open(), Encoding.Unicode);
                fileVersion = reader.ReadToEnd();
            }

            return fileVersion.Trim();
        }
    }
}
