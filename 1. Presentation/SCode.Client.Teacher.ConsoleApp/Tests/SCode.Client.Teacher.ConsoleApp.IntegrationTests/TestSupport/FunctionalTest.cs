using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Xunit;

namespace SCode.Client.Teacher.ConsoleApp.IntegrationTests.TestSupport
{
    public abstract class FunctionalTest
        : IAsyncLifetime
    {
        private readonly IServiceProvider _serviceProvider;

        // protected HttpClient HttpClient { get; }
        protected IConfiguration Configuration { get; }

        protected FunctionalTest()
        {
            Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Test");

            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureLogging((_, loggingBuilder) =>
                {
                    loggingBuilder.ClearProviders();

                    var logger = new LoggerConfiguration()
                        .MinimumLevel.Debug()
                        .WriteTo.Console()
                        .CreateLogger();

                    loggingBuilder.AddSerilog(logger, dispose: true);
                })
                .ConfigureServices((_, services) =>
                    ConfigureTestServices(services));

            var host = hostBuilder.Build();

            _serviceProvider = host.Services;

            Configuration = _serviceProvider.GetService<IConfiguration>();
        }

        protected T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        protected virtual void ConfigureTestServices(IServiceCollection services)
        {
            services.AddApplication();
        }

        protected virtual void Cleanup()
        {
        }

        protected string GetResourcesPath(params string[] paths)
        {
            var listPath = paths.ToList();
            listPath.Insert(0, "Resources");
            
            return Path.Combine(listPath.ToArray());
        }

        protected abstract Task Given();

        protected abstract Task When();

        public async Task InitializeAsync()
        {
            await Given();
            await When();
        }

        public Task DisposeAsync()
        {
            Cleanup();
            return Task.CompletedTask;
        }
    }
}