using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Extensions;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Steps
{
    /// <summary>
    /// Registrar los cambios
    /// </summary>
    public class FileChangePipelinePipelineLogStep : IFileChangePipelineLogStep
    {
        private readonly IFileChangePipelineBatchChangesStep _batchChangesStep;
        private readonly ILogger<FileChangePipelinePipelineLogStep> _logger;

        public FileChangePipelinePipelineLogStep(ILogger<FileChangePipelinePipelineLogStep> logger, IFileChangePipelineBatchChangesStep batchChangesStep)
        {
            _logger = logger;
            _batchChangesStep = batchChangesStep;
        }

        public Task Execute(FileSystemEntryChange change)
        {
            _logger.LogDebug("{Change} {Path} {OldPath}",
                change.ChangeType.ToEsName(), change.Path, change.OldPath);

            return _batchChangesStep.Execute(change);
        }
    }

    public interface IFileChangePipelineLogStep
    {
        Task Execute(FileSystemEntryChange change);
    }
}