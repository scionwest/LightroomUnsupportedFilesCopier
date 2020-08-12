using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;

namespace LightroomPhotoCopy
{
    [Command(Name = "fm", FullName = "photocopy", UnrecognizedArgumentHandling = UnrecognizedArgumentHandling.Throw, OptionsComparison = StringComparison.InvariantCultureIgnoreCase)]
    [VersionOptionFromMember("--version", MemberName = nameof(Version))]
    [Subcommand(typeof(CopyFromFileCommand), typeof(DeleteFilesCommand))]
    public class LightroomCopyCommand : CommandBase
    {
        public LightroomCopyCommand(IConsole console) : base(console)
        {
        }

        protected override Task<int> Execute(CommandLineApplication app)
        {
            app.ShowHelp();
            return Task.FromResult(0);
        }
    }
}
