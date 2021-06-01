using D4.PowerBI.Meta.Constants;
using D4.PowerBI.Meta.Exceptions;
using D4.PowerBI.Meta.Models;
using System.Linq;
using System.Text.Json;
using System.Text;
using System.IO;

namespace D4.PowerBI.Meta.Read
{
    public static class DiagramLayoutReader
    {
        public static DiagramLayout? ReadDiagramLayout(this PBIFile pbiFile)
        {
            var layoutFile = pbiFile.ArchiveEntries
                .FirstOrDefault(x => x.FullName == PbiFileContents.DiagramLayout);

            if (layoutFile == null)
            {
                throw new ContentNotFoundException("Unavle to read Diagram Layout content.");
            }

            var reader = new StreamReader(layoutFile.Open(), Encoding.Unicode);
            var layoutFileContent = reader.ReadToEnd();

            if (layoutFileContent == null)
            {
                throw new ContentEmptyException("Diagram Layout is empty");
            }

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<DiagramLayout>(layoutFileContent, options);
        }
    }
}
