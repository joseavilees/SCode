using System.Collections.Generic;
using System.Threading.Tasks;
using SCode.Client.Teacher.ConsoleApp.Application.Helpers;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Steps
{
    public class FileChangePipelineMergeChangesStep : IFileChangePipelineMergeChangesStep
    {
        private readonly IFileChangePipelineTransformChangeToLogicStep _transformChangeToLogicStep;

        public FileChangePipelineMergeChangesStep(IFileChangePipelineTransformChangeToLogicStep transformChangeToLogicStep)
        {
            _transformChangeToLogicStep = transformChangeToLogicStep;
        }

        public Task Execute(List<FileSystemEntryChange> input)
        {
            var output = FileSystemEntryChangeHelper
                .Merge(input);

            return _transformChangeToLogicStep
                .Execute(output);
        }
    }

    public interface IFileChangePipelineMergeChangesStep
    {
        Task Execute(List<FileSystemEntryChange> input);
    }
}