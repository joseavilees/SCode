using System;
using SCode.Client.Teacher.ConsoleApp.Application.Helpers;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.AppFileEntities;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Application.Mappers
{
    public static class FileEntryChangeMapper
    {
        public static AppFileEntryChange MapToAppFileEntryChange(FileSystemEntryChange fileChange, int id, int parentId)
        {
            var isDirectory = PathHelper
                .IsDirectoryPath(fileChange.Path);

            var appFileEntry = new AppFileEntry(id, parentId,
                fileChange.Name, isDirectory);

            var appFileChangeType = MapToAppFileEntryChangeType(fileChange.ChangeType);

            var content = GetContent(fileChange, isDirectory);

            var appFileChange = new AppFileEntryChange(appFileEntry,
                appFileChangeType, content);

            return appFileChange;
        }

        private static string GetContent(FileSystemEntryChange fileChange, bool isDirectory)
        {
            if (isDirectory || fileChange.ChangeType != FileSystemEntryChangeType.Changed)
                return null;

            var content = FileHelper
                .GetFileContent(fileChange.Path);

            return content;
        }


        public static AppFileEntryChangeType MapToAppFileEntryChangeType(FileSystemEntryChangeType src) =>
            Enum.Parse<AppFileEntryChangeType>(src.ToString());
    }
}