using McMaster.Extensions.CommandLineUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace LightroomPhotoCopy
{
    [Command(Name = "copy", Description = "Copies all of the files included in a saved Import result text file that were unsupported by Lightroom.", UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw, OptionsComparison = StringComparison.InvariantCultureIgnoreCase)]
    public class CopyFromFileCommand : CommandBase
    {
        private readonly IFileManager fileManager;
        private ICollection<string> skippedFiles = new List<string>();
        private int copiedCount = 0;

        public CopyFromFileCommand(IConsole console, IFileManager fileManager) : base(console)
        {
            this.fileManager = fileManager;
        }

        [Option("-f|--file", CommandOptionType.SingleValue, Description = "The source file saved after the import result was completed", LongName = "file", ShortName = "f")]
        [Required]
        public string File { get; set; }

        [Option("-o|--output", CommandOptionType.SingleValue, Description = "The output folder that the files will be copied to. If not provided then a folder matching the source filename will be created in the same directory as the source file.", LongName = "output", ShortName = "o")]
        public string OutputPath { get; set; }

        [Option("-d|--delete", CommandOptionType.NoValue, Description = "Deletes the original file after a copy is completed.", LongName = "delete", ShortName = "d")]
        public bool DeleteFile { get; set; }

        protected override Task<int> Execute(CommandLineApplication app)
        {
            string[] individualFiles;

            try
            {
                this.PrepareOutputPath();
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

            base.WriteInfoToConsole($"Copying files into the {this.OutputPath} directory.");
            foreach (string file in individualFiles)
            {
                try
                {
                    this.CopyFile(file);
                }
                catch (Exception)
                {
                    base.WriteErrorToConsole($"Failed to process the {file}. Aborting with {this.copiedCount} files copied.");
                    return Task.FromResult(1);
                }
            }

            base.WriteInfoToConsole("Following files were skipped due to appearing as potential duplicates.");
            base.WriteInfoToConsole(string.Join("\n", this.skippedFiles));
            base.WriteInfoToConsole($"{this.skippedFiles.Count} files were skipped.");
            base.WriteInfoToConsole($"Copied {this.copiedCount} out of {individualFiles.Length}.");
            base.WriteInfoToConsole("Copy completed");
            if (this.DeleteFile)
            {
                base.WriteInfoToConsole("Original files deleted.");
            }

            return Task.FromResult(0);
        }

        private void CopyFile(string file)
        {
            var sourceFile = new FileInfo(file);
            var destinationFile = new FileInfo($"{this.OutputPath}\\{sourceFile.Name}");

            if (System.IO.File.Exists(destinationFile.FullName))
            {
                if (sourceFile.Length != destinationFile.Length)
                {
                    base.WriteErrorToConsole($"The {destinationFile.FullName} already exists and appears to be different and will be renamed!");
                    string newFilename = $"{destinationFile.Name.Substring(0, destinationFile.Name.Length - destinationFile.Extension.Length)}-{Guid.NewGuid()}.{destinationFile.Extension}";
                    destinationFile = new FileInfo($"{this.OutputPath}\\{newFilename}");
                }
                else
                {
                    this.skippedFiles.Add(file);
                    return;
                }
            }

            base.WriteInfoToConsole($"Copying file {sourceFile.FullName}");
            System.IO.File.Copy(sourceFile.FullName, destinationFile.FullName);
            this.copiedCount++;

            if (this.DeleteFile)
            {
                base.WriteInfoToConsole($"Copying completed. Deletion for file {destinationFile.Name} processing.");
                System.IO.File.Delete(sourceFile.FullName);
            }
        }

        private void PrepareOutputPath()
        {
            base.WriteInfoToConsole($"Reading from {this.File}");
            FileInfo inputFile = new FileInfo(this.File);
            if (!System.IO.File.Exists(inputFile.FullName))
            {
                throw new FileNotFoundException();
            }

            DirectoryInfo inputDirectory = new DirectoryInfo(inputFile.DirectoryName);

            if (string.IsNullOrEmpty(this.OutputPath))
            {
                string workingFolderName = inputFile.Name.Substring(0, inputFile.Name.Length - inputFile.Extension.Length);
                this.OutputPath = $"{inputDirectory.FullName}\\{workingFolderName}";
            }

            if (this.OutputPath.EndsWith("\\"))
            {
                this.OutputPath = this.OutputPath.Substring(0, this.OutputPath.Length - 1);
            }

            if (!Directory.Exists(this.OutputPath))
            {
                Directory.CreateDirectory(this.OutputPath);
            }
        }
    }
}
