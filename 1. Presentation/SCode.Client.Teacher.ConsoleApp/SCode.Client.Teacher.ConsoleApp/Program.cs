using Serilog;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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

                    var environmentName = hostContext.HostingEnvironment.EnvironmentName;
                    var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddJsonFile($"appsettings.{environmentName}.json",
                            optional: true)
                        .AddEnvironmentVariables()
                        .Build();

                    var logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(configuration)
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