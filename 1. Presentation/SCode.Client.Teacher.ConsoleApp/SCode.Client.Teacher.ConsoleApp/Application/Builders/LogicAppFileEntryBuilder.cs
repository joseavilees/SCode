using System.Collections.Generic;
using System.IO;
using System.Linq;
using SCode.Client.Teacher.ConsoleApp.Application.Services;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.AppFileEntities;
using SCode.Client.Teacher.ConsoleApp.Domain.Repositories;

namespace SCode.Client.Teacher.ConsoleApp.Application.Builders
{
    /// <summary>
    /// Transforma los archivos del sistema en archivos
    /// lógicos identificados con un Id
    /// </summary>
    public class LogicAppFileEntryBuilder : ILogicAppFileEntryBuilder
    {
        private readonly ILogicPathRepository _logicPathRepository;
        private readonly IGitIgnoreService _gitIgnoreService;

        public LogicAppFileEntryBuilder(ILogicPathRepository logicPathRepository,
            IGitIgnoreService gitIgnoreService)
        {
            _logicPathRepository = logicPathRepository;
            _gitIgnoreService = gitIgnoreService;
        }

        public List<AppFileEntry> Build(string path)
        {
            var parentId = _logicPathRepository
                .Add(path);

            return Traverse(path, parentId)
                .ToList();
        }

        private IEnumerable<AppFileEntry> Traverse(string path, int parentId)
        {
            var dirInfo = new DirectoryInfo(path);

            var files = dirInfo.GetFiles();
            var directories = dirInfo.GetDirectories();

            foreach (var file in files)
            {
                if (_gitIgnoreService.IsIgnored(file))
                    continue;
                
                var id = _logicPathRepository
                    .Add(file.FullName);

                yield return new AppFileEntry(id, parentId,
                    file.Name, false);
            }

            foreach (var directory in directories)
            {
                if (_gitIgnoreService.IsIgnored(directory))
                    continue;
                
                var id = _logicPathRepository
                    .Add(directory.FullName);

                yield return new AppFileEntry(id, parentId,
                    directory.Name, true);

                foreach (var child in Traverse(directory.FullName, id))
                    yield return child;
            }
        }
    }

    public interface ILogicAppFileEntryBuilder
    {
        List<AppFileEntry> Build(string path);
    }
}