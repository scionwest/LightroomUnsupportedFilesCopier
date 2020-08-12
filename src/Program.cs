using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace LightroomPhotoCopy
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {

            return await new HostBuilder()
                .ConfigureServices((builderContext, services) => services.AddSingleton<IFileManager, FileManager>())
                .RunCommandLineApplicationAsync<LightroomCopyCommand>(args);
        }
    }
}
