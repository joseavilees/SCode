using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Filters;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Steps
{
    /// <summary>
    /// Agrupa todos los cambios producidos en 300 MS
    /// </summary>
    public class FileChangePipelineBatchChangesStep : IFileChangePipelineBatchChangesStep
    {
        public static readonly TimeSpan TriggerPeriod = TimeSpan.FromMilliseconds(300);

        private readonly SemaphoreSlim _semaphore;

        private readonly IFileChangePipelineMergeChangesStep _mergeChangesStep;
        private readonly ILogger<FileChangePipelineBatchChangesStep> _logger;
        private readonly List<FileSystemEntryChange> _buffer;

        private readonly IFileChangePipelinePathFilter _pathFilter;

        public FileChangePipelineBatchChangesStep(IFileChangePipelineMergeChangesStep mergeChangesStep,
            ILogger<FileChangePipelineBatchChangesStep> logger,
            IFileChangePipelinePathFilter pathFilter)
        {
            _logger = logger;
            _pathFilter = pathFilter;
            _mergeChangesStep = mergeChangesStep;

            _semaphore = new SemaphoreSlim(1, 1);

            _buffer = new List<FileSystemEntryChange>();

            Task.Run(TriggerBatchBackgroundThread);
        }

        public async Task Execute(FileSystemEntryChange change)
        {
            if (!_pathFilter.IsValid(change.Path))
                return;

            await _semaphore.WaitAsync();

            try
            {
                _buffer.Add(change);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task TriggerBatchBackgroundThread()
        {
            while (true)
            {
                await TriggerBatch();
                
                await Task.Delay(TriggerPeriod);
            }
        }
        private async Task TriggerBatch()
        {
            await _semaphore.WaitAsync();

            try
            {
                if (_buffer.Count == 0)
                    return;

                var output = _buffer.ToList();

                await _mergeChangesStep
                    .Execute(output);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "No fue posible lanzar el conjunto de cambios {@Buffer}", _buffer);
            }
            finally
            {
                _semaphore.Release();
                
                _buffer.Clear();
            }
        }
    }

    public interface IFileChangePipelineBatchChangesStep
    {
        Task Execute(FileSystemEntryChange change);
    }
}