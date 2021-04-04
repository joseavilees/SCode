using System;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Application.Extensions
{
    public static class FileSystemEntryChangeTypeExtension
    {
        /// <summary>
        /// Formatea el cambio al castellano
        /// </summary>
        /// <param name="changeType"></param>
        /// <returns></returns>
        public static string ToEsName(this FileSystemEntryChangeType changeType)
        {
            return changeType switch
            {
                FileSystemEntryChangeType.Changed => "* Actualizado: ",
                FileSystemEntryChangeType.Created => "+ Creado: ",
                FileSystemEntryChangeType.Renamed => "* Renombrado: ",
                FileSystemEntryChangeType.Deleted => "- Eliminado: ",

                _ => throw new ArgumentOutOfRangeException(nameof(changeType),
                    changeType, null)
            };
        }
    }
}