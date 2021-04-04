using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SCode.Client.Teacher.ConsoleApp.Application.Mappers;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.AppFileEntities;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;
using SCode.Client.Teacher.ConsoleApp.Domain.Repositories;

namespace SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Steps
{
    /// <summary>
    /// Convierte el cambio del sistema de archivos a cambio lógico
    /// </summary>
    public class FileChangePipelineTransformChangeToLogicStep : IFileChangePipelineTransformChangeToLogicStep
    {
        private readonly IFileChangePipelinePublishChangesStep _publishChangesStep;
        private readonly ILogicPathRepository _logicPathRepository;

        public FileChangePipelineTransformChangeToLogicStep(ILogicPathRepository logicPathRepository,
            IFileChangePipelinePublishChangesStep publishChangesStep)
        {
            _logicPathRepository = logicPathRepository;
            _publishChangesStep = publishChangesStep;
        }

        public Task Execute(List<FileSystemEntryChange> input)
        {
            var output = input
                    .Select(ConvertToAppFileChange)
                    .ToList();

            return _publishChangesStep
                .Execute(output);
        }

        private AppFileEntryChange ConvertToAppFileChange(FileSystemEntryChange change)
        {
            var path = change.ChangeType == FileSystemEntryChangeType.Renamed
                ? change.OldPath
                : change.Path;

            var id = _logicPathRepository.Add(path);
            if (change.ChangeType == FileSystemEntryChangeType.Renamed)
                _logicPathRepository.Update(id, change.Path);

            var parentId = GetParentId(path);

            var appFileChange = FileEntryChangeMapper
                .MapToAppFileEntryChange(change, id, parentId);

            return appFileChange;
        }

        private int GetParentId(string path)
        {
            var parent = Directory.GetParent(path);
            if (parent == null)
                throw new NotSupportedException(
                    $"No se encontró un padre para la ruta {path}");

            var id = _logicPathRepository
                .Add(parent.FullName);

            return id;
        }
    }

    public interface IFileChangePipelineTransformChangeToLogicStep
    {
        Task Execute(List<FileSystemEntryChange> input);
    }
}