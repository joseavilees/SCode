using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Clients;
using SCode.Client.Teacher.ConsoleApp.Application.Helpers;
using SCode.Client.Teacher.ConsoleApp.Application.Services;

namespace SCode.Client.Teacher.ConsoleApp
{
    public class ConsoleApp : IHostedService
    {
        private readonly IClassroomHubClient _classroomHubClient;
        private readonly IAccountService _accountService;
        private readonly ISessionService _sessionService;
        private readonly ILogger<ConsoleApp> _logger;

        public ConsoleApp(IClassroomHubClient classroomHubClient, IAccountService accountService,
            ISessionService sessionService, ILogger<ConsoleApp> logger)
        {
            _classroomHubClient = classroomHubClient;
            _accountService = accountService;
            _sessionService = sessionService;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            ConsoleHelper.PrintWelcome();

            _logger.LogInformation("Aplicación iniciada");

            if (!await Login())
                return;

            var classroomName = RequestClassroomName();
            _sessionService
                .SetClassroomName(classroomName);

            var baseDirectoryPath = RequestBaseDirectoryPath();
            _sessionService
                .SetBaseDirectoryPath(baseDirectoryPath);

            await _classroomHubClient
                .Connect();

            ConsoleHelper.WriteBlackLine();
        }

        private static string RequestBaseDirectoryPath()
        {
            string baseDirectoryPath = null;
            while (!Directory.Exists(baseDirectoryPath))
            {
                Console.WriteLine("Introduce el directorio:");
                baseDirectoryPath = Console.ReadLine();
            }

            return baseDirectoryPath;
        }

        private string RequestClassroomName()
        {
            string classRoomName = null;
            while (string.IsNullOrWhiteSpace(classRoomName))
            {
                Console.WriteLine("Introduce nombre de la clase:");
                classRoomName = Console.ReadLine();
            }

            ConsoleHelper.WriteBlackLine();

            return classRoomName;
        }

        private async Task<bool> Login()
        {
            if (!await _accountService.Login())
            {
                _logger.LogError("No fue posible autentificar compruebe que ha " +
                                 "establecido la API-Key en el archivo appSettings.json y vuelva " +
                                 "a abrir la aplicación");

                return false;
            }

            return true;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}