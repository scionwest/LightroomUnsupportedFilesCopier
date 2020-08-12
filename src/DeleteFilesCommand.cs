using McMaster.Extensions.CommandLineUtils;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace LightroomPhotoCopy
{
    [Command(Name = "delete", Description = "Deletes the files found in a Lightroom Import results file.", UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw, OptionsComparison = StringComparison.InvariantCultureIgnoreCase)]
    public class DeleteFilesCommand : CommandBase
    {
        const string unsupportedFilesText = "These files appear to be unsupported or damaged";
        private readonly IFileManager fileManager;

        public DeleteFilesCommand(IConsole console, IFileManager fileManager) : base(console)
        {
            this.fileManager = fileManager;
        }

        [Option("-f|--file", CommandOptionType.SingleValue, Description = "The title of the project being created", LongName = "file", ShortName = "f")]
        [Required]
        public string File { get; set; }

        protected override Task<int> Execute(CommandLineApplication app)
        {
            string[] individualFiles;

            try
            {
                individualFiles = this.fileManager.GetFilesFromLightroomExport(this.File);
            }
            catch (FileNotFoundException)
            {
                base.WriteErrorToConsole($"Unable to locate the file {this.File}.");
                return Task.FromResult(1);
            }
            catch (NoUnsupportedFilesFoundException ex)
            {
                base.WriteErrorToConsole(ex.Message);
                return Task.FromResult(1);
            }

            int filesDeleted = 0;
            foreach (string file in individualFiles)
            {
                var sourceFile = new FileInfo(file);

                base.WriteInfoToConsole($"Deleting file {sourceFile.Name}");
                try
                {
                    System.IO.File.Delete(sourceFile.FullName);
                }
                catch (Exception)
                {
                    base.WriteErrorToConsole($"Failed to process the {file}. Aborting with {filesDeleted} files deleted.");
                    return Task.FromResult(1);
                }
                filesDeleted++;
            }

            base.WriteInfoToConsole($"Deleted {filesDeleted} out of {individualFiles.Length}.");
            base.WriteInfoToConsole("Delete completed");
            return Task.FromResult(0);
        }
    }
}
