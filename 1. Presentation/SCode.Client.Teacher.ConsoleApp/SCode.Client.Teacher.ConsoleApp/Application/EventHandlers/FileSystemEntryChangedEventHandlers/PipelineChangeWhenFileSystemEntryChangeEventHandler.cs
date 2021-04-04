using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline;
using SCode.Client.Teacher.ConsoleApp.Domain.Events;

namespace SCode.Client.Teacher.ConsoleApp.Application.EventHandlers.FileSystemEntryChangedEventHandlers
{
    public class PipelineChangeWhenFileSystemEntryChangeEventHandler : INotificationHandler<FileSystemEntryChangedEvent>
    {
        private readonly ILogger<PipelineChangeWhenFileSystemEntryChangeEventHandler> _logger;
        private readonly IFileChangePipeline _fileChangePipeline;

        public PipelineChangeWhenFileSystemEntryChangeEventHandler(
            ILogger<PipelineChangeWhenFileSystemEntryChangeEventHandler> logger,
            IFileChangePipeline fileChangePipeline)
        {
            _fileChangePipeline = fileChangePipeline;
            _logger = logger;
        }

        public Task Handle(FileSystemEntryChangedEvent notification, CancellationToken cancellationToken)
        {
            var change = notification.Change;
            try
            {
                return _fileChangePipeline
                    .Execute(change);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "No fue posible canalizar el cambio {@Change}", change);

                return Task.CompletedTask;
            }      
        }
    }
}