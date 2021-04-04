using System;
using System.IO;
using MAB.DotIgnore;
using Microsoft.Extensions.Logging;
using SCode.Client.Teacher.ConsoleApp.Application.Helpers;

namespace SCode.Client.Teacher.ConsoleApp.Application.Services
{
    public class GitIgnoreService : IGitIgnoreService
    {
        private readonly IgnoreList _ignoreList;

        private readonly bool _isInitialized;

        public GitIgnoreService(ILogger<GitIgnoreService> logger)
        {
            try
            {
                _ignoreList = new IgnoreList();

                var gitIgnoreFiles = Directory.GetFiles(@".\Resources\GitIgnores");
                foreach (var gitIgnoreFile in gitIgnoreFiles)
                    _ignoreList.AddRules(gitIgnoreFile);

                _isInitialized = true;

                logger.LogInformation("GitIgnore inicializado con {Files}",
                    gitIgnoreFiles.Length);
            }
            catch (Exception ex)
            {
                _isInitialized = false;

                logger.LogError(ex, "No fue posible inicializar GitIgnore");
            }
        }

        public bool IsIgnored(string path) =>
            !_isInitialized || _ignoreList.IsIgnored(path, PathHelper.IsDirectoryPath(path));

        public bool IsIgnored(FileInfo fileInfo)
            => !_isInitialized || _ignoreList.IsIgnored(fileInfo);

        public bool IsIgnored(DirectoryInfo directoryInfo)
            => !_isInitialized || _ignoreList.IsIgnored(directoryInfo);
    }

    public interface IGitIgnoreService
    {
        bool IsIgnored(string path);
        bool IsIgnored(FileInfo fileInfo);
        bool IsIgnored(DirectoryInfo directoryInfo);
    }
}