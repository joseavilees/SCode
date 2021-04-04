using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Services;
using SCode.Client.Teacher.ConsoleApp.Domain.Events;

namespace SCode.Client.Teacher.ConsoleApp.Application.EventHandlers.ClassroomStartedEventHandlers
{
    public class StartDirectoryWatcherServiceWhenClassroomStartedEventHandler
        : INotificationHandler<ClassroomStartedEvent>
    {
        private readonly ILogger<StartDirectoryWatcherServiceWhenClassroomStartedEventHandler> _logger;
        private readonly IDirectoryWatcherService _directoryWatcherService;
        private readonly ISessionService _sessionService;

        public StartDirectoryWatcherServiceWhenClassroomStartedEventHandler(
            IDirectoryWatcherService directoryWatcherService,
            ISessionService sessionService,
            ILogger<StartDirectoryWatcherServiceWhenClassroomStartedEventHandler> logger)
        {
            _directoryWatcherService = directoryWatcherService;
            _sessionService = sessionService;
            _logger = logger;
        }

        public Task Handle(ClassroomStartedEvent notification, CancellationToken cancellationToken)
        {
            var path = _sessionService.BaseDirectoryPath;
            try
            {
                _directoryWatcherService
                    .Start(path);

                _logger.LogInformation("Iniciado el escaneo de cambios en {Path}", path);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "No fue posible iniciar el escaneo de cambios en {Path}", path);
            }
            
            return Task.CompletedTask;
        }
    }
}