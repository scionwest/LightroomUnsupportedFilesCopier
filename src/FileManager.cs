using System.IO;
using System.Linq;

namespace LightroomPhotoCopy
{
    internal class FileManager : IFileManager
    {
        const string unsupportedFilesText = "These files appear to be unsupported or damaged";

        public string[] GetFilesFromLightroomExport(string file)
        {
            if (!System.IO.File.Exists(file))
            {
                throw new FileNotFoundException();
            }

            string fileContent = System.IO.File.ReadAllText(file);
            string splitContent = fileContent.Split("\r\n\r\n").First(content => content.StartsWith(unsupportedFilesText));
            if (string.IsNullOrEmpty(splitContent))
            {
                throw new NoUnsupportedFilesFoundException("The file provided does not have any files denoted as being unsupported.");
            }

            string[] individualLines = splitContent.Split('\n')
                        .Select(file => file.Trim())
                        .ToArray();
            if (individualLines.Length < 2)
            {
                throw new NoUnsupportedFilesFoundException("File does not contain files that need to be copied.");
            }

            string[] individualFiles = individualLines
                .Skip(1)
                .Select(file => file.Trim())
                .Where(file => !string.IsNullOrWhiteSpace(file))
                .ToArray();

            if (individualFiles.Length == 0)
            {
                throw new NoUnsupportedFilesFoundException("Failed to locate the unsupported content section and produce a list of files to copy.");
            }

            return individualFiles;
        }
    }
}
