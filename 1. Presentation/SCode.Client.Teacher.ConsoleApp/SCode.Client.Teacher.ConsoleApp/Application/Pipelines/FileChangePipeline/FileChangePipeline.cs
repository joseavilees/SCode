using System.Threading.Tasks;
using SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Steps;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline
{
    public class FileChangePipeline : IFileChangePipeline
    {
        private readonly IFileChangePipelineLogStep _logStep;

        public FileChangePipeline(IFileChangePipelineLogStep logStep)
        {
            _logStep = logStep;
        }

        public Task Execute(FileSystemEntryChange input)
        {
            return _logStep.Execute(input);
        }
    }

    public interface IFileChangePipeline
    {
        Task Execute(FileSystemEntryChange input);
    }
}