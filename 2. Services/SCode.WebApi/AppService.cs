using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SCode.WebApi
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;

        public AppService(ILogger<AppService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Aplicación iniciada");
                
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(500, stoppingToken);
                }
            }
            finally
            {
                _logger.LogCritical("Deteniendo aplicación...");
            }
        }
    }
}