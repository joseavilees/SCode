using System;
using System.Collections.Generic;
using System.Linq;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Application.Helpers
{
    public static class FileSystemEntryChangeHelper
    {
        /// <summary>
        /// Fusiona los cambios innecesarios
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static List<FileSystemEntryChange> Merge(IEnumerable<FileSystemEntryChange> src)
        {
            // Un archivo ha sido creado y después eliminado
            Func<FileSystemEntryChange, bool> fileSystemEntryCreatedAndThenDeletedPredicate =
                x => x.ChangeType == FileSystemEntryChangeType.Created &&
                     src.Any(a=> a.Path == x.Path && a.ChangeType == FileSystemEntryChangeType.Deleted);
            
            
            return src
                .Where(x=> !fileSystemEntryCreatedAndThenDeletedPredicate(x))
                .Distinct()
                .ToList();
        }
    }
}