using System.IO;
using System.Linq;
using System.Text;

namespace D4.PowerBI.Meta.Content
{
    public static class FileVersion
    {
        public static string ReadFileVersion(this PBIFile pbiFile)
        {
            var fileVersion = string.Empty;

            var versionFile = pbiFile.ArchiveEntries.FirstOrDefault(x => x.FullName == "Version");
            if (versionFile != null)
            {
                var reader = new StreamReader(versionFile.Open(), Encoding.Unicode);
                fileVersion = reader.ReadToEnd();
            }

            return fileVersion.Trim();
        }
    }
}
