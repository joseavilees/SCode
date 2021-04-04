using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Flurl;
using MediatR;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Builders;
using SCode.Client.Teacher.ConsoleApp.Application.Clients;
using SCode.Client.Teacher.ConsoleApp.Application.Mappers;
using SCode.Client.Teacher.ConsoleApp.Application.Services;
using SCode.Client.Teacher.ConsoleApp.Domain.Events;
using SCode.Shared.Requests;

namespace SCode.Client.Teacher.ConsoleApp.Application.EventHandlers.ClassroomHubClientConnectedEventHandlers
{
    public class StartClassroomWhenClassroomHubClientConnectedEventHandler
        : INotificationHandler<ClassroomHubClientConnectedEvent>
    {
        private readonly ILogger<StartClassroomWhenClassroomHubClientConnectedEventHandler> _logger;
        private readonly ILogicAppFileEntryBuilder _logicAppFileEntryBuilder;
        private readonly IClassroomHubClient _classroomHubClient;
        private readonly ISessionService _sessionService;
        private readonly AppSettings _appSettings;
        private readonly IMediator _bus;

        public StartClassroomWhenClassroomHubClientConnectedEventHandler(
            ILogger<StartClassroomWhenClassroomHubClientConnectedEventHandler> logger,
            ILogicAppFileEntryBuilder logicAppFileEntryBuilder, IClassroomHubClient classroomHubClient,
            ISessionService sessionService, AppSettings appSettings, IMediator bus)
        {
            _logger = logger;
            _logicAppFileEntryBuilder = logicAppFileEntryBuilder;
            _classroomHubClient = classroomHubClient;
            _sessionService = sessionService;
            _appSettings = appSettings;
            _bus = bus;
        }

        public async Task Handle(ClassroomHubClientConnectedEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var request = CreateStartClassroomRequest();

                await _classroomHubClient
                    .SendAsync("StartClassroom", request);

                var classRoomUrl = GetClassroomUrl();

                await _bus.Publish(new ClassroomStartedEvent(),
                    cancellationToken);

                _logger
                    .LogInformation("Clase iniciada en: {Url}", classRoomUrl);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No fue posible iniciar la sala");
            }
        }

        private Url GetClassroomUrl()
        {
            var classroomName = _sessionService
                .ClassroomName;

            var classRoomUrl = new Url(_appSettings.SCodeStudentWebApp)
                .AppendPathSegment(classroomName);

            return classRoomUrl;
        }

        private StartClassroomRequest CreateStartClassroomRequest()
        {
            var classroomName = _sessionService
                .ClassroomName;

            var rootPath = _sessionService
                .BaseDirectoryPath;

            var appFileEntries = _logicAppFileEntryBuilder
                .Build(rootPath);

            var appFileEntriesDto = appFileEntries
                .Select(AppFileEntryMapper.MapToAppFileDto)
                .ToList();

            var request = new StartClassroomRequest(classroomName,
                appFileEntriesDto);

            return request;
        }
    }
}