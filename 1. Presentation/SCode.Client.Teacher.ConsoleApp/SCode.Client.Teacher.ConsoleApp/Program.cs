using Serilog;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SCode.Client.Teacher.ConsoleApp
{
    static class Program
    {
        private static async Task Main(string[] args)
        {
            await Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostContext, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();

                    var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(hostContext.Configuration)
                        .CreateLogger();

                    loggingBuilder.AddSerilog(logger, dispose: true);
                })
                .ConfigureServices((_, services) =>
                    services
                        .AddApplication()
                        .AddHostedService<ConsoleApp>())
                .RunConsoleAsync();
        }
    }
}