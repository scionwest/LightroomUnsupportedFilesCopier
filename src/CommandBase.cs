using McMaster.Extensions.CommandLineUtils;
using System;
using System.Threading.Tasks;

namespace LightroomPhotoCopy
{
    public abstract class CommandBase
    {
        public CommandBase(IConsole console)
        {
            Console = console ?? throw new System.ArgumentNullException(nameof(console));
        }

        public IConsole Console { get; }

        protected string Version => "1.0.0";

        public Task<int> OnExecute(CommandLineApplication app)
        {
            return this.Execute(app);
        }

        protected abstract Task<int> Execute(CommandLineApplication app);

        protected void WriteInfoToConsole(string message)
        {
            this.Console.BackgroundColor = ConsoleColor.Black;
            this.Console.ForegroundColor = ConsoleColor.Yellow;
            this.Console.Out.WriteLine(message);
            this.Console.ResetColor();
        }

        protected void WriteErrorToConsole(string message)
        {
            this.Console.BackgroundColor = ConsoleColor.Red;
            this.Console.ForegroundColor = ConsoleColor.White;
            this.Console.Error.WriteLine(message);
            this.Console.ResetColor();
        }
    }
}
