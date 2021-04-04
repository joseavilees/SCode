using System.IO;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Application.Mappers
{
    public class FileSystemEventArgsMapper
    {
        public static FileSystemEntryChange MapToAppFileSystemEntryChange(FileSystemEventArgs src)
        {
            var name = Path.GetFileName(src.FullPath);
            var path = src.FullPath;
            var oldPath = "";
            var changeType = WatcherChangeTypesMapper
                .MapToFileSystemEntryChangeType(src.ChangeType);

            if (src.ChangeType == WatcherChangeTypes.Renamed)
            {
                var fileChange = src as RenamedEventArgs;

                oldPath = fileChange.OldFullPath;
            }

            return new FileSystemEntryChange(changeType, name, path, oldPath);
        }
    }
}