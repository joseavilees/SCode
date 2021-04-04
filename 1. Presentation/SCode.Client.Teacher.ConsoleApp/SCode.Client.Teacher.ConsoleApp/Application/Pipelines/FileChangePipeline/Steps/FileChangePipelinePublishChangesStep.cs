using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Clients;
using SCode.Client.Teacher.ConsoleApp.Application.Mappers;
using SCode.Client.Teacher.ConsoleApp.Application.Services;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.AppFileEntities;
using SCode.Shared.Requests;

namespace SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Steps
{
    public class FileChangePipelinePublishChangesStep : IFileChangePipelinePublishChangesStep
    {
        private readonly ILogger<FileChangePipelinePublishChangesStep> _logger;
        private readonly IClassroomHubClient _classroomHubClient;
        private readonly ISessionService _sessionService;

        public FileChangePipelinePublishChangesStep(ILogger<FileChangePipelinePublishChangesStep> logger,
            IClassroomHubClient classroomHubClient, ISessionService sessionService)
        {
            _logger = logger;
            _classroomHubClient = classroomHubClient;
            _sessionService = sessionService;
        }

        public async Task Execute(List<AppFileEntryChange> changes)
        {
            try
            {
                var changesDto = changes
                    .Select(AppFileEntryMapper.MapToAppFileEntryChangeDto)
                    .ToList();

                var request = new AppFileEntryChangeRequest
                {
                    ClassRoomName = _sessionService.ClassroomName,
                    Changes = changesDto
                };

                await _classroomHubClient
                    .SendAsync("NotifyAppFileEntryChange", request);

                _logger.LogInformation("Enviados {Count} al concentrador",
                    changesDto.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "No fue posible enviar al servidor " +
                                     "los cambios {@Changes}", changes);
            }   
        }
    }

    public interface IFileChangePipelinePublishChangesStep
    {
        Task Execute(List<AppFileEntryChange> changes);
    }
}