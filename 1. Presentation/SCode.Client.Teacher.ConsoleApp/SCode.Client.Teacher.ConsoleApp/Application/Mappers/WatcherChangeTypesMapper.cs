using System;
using System.IO;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Application.Mappers
{
    public class WatcherChangeTypesMapper
    {
        public static FileSystemEntryChangeType MapToFileSystemEntryChangeType(WatcherChangeTypes src) =>
            Enum.Parse<FileSystemEntryChangeType>(src.ToString());
    }
}