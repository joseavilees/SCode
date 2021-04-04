using SCode.Client.Teacher.ConsoleApp.Application.Helpers;
using SCode.Client.Teacher.ConsoleApp.Application.Services;

namespace SCode.Client.Teacher.ConsoleApp.Application.Pipelines.FileChangePipeline.Filters
{
    /// <summary>
    /// Filtra rutas válidas
    /// </summary>
    public class FileChangePipelinePathFilter : IFileChangePipelinePathFilter
    {
        private readonly IGitIgnoreService _gitIgnoreService;

        public FileChangePipelinePathFilter(IGitIgnoreService gitIgnoreService)
        {
            _gitIgnoreService = gitIgnoreService;
        }

        public bool IsValid(string path) => 
            !PathHelper.IsBackupFile(path) && !_gitIgnoreService.IsIgnored(path);
    }

    public interface IFileChangePipelinePathFilter
    {
        bool IsValid(string path);
    }
}