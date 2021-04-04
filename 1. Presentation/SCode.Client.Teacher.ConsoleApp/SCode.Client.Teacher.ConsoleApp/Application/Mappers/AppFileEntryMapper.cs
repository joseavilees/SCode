using System;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.AppFileEntities;
using SCode.Shared.Dtos.AppFile;

namespace SCode.Client.Teacher.ConsoleApp.Application.Mappers
{
    public class AppFileEntryMapper
    {
        public static AppFileEntryDto MapToAppFileDto(AppFileEntry src)
        {
            return new()
            {
                Id = src.Id,
                ParentId = src.ParentId,
                Name = src.Name,
                IsDirectory = src.IsDirectory
            };
        }

        public static AppFileEntryChangeTypeDto MapToAppFileChangeTypeDto(AppFileEntryChangeType src) =>
            Enum.Parse<AppFileEntryChangeTypeDto>(src.ToString());

        public static AppFileEntryChangeDto MapToAppFileEntryChangeDto(AppFileEntryChange src)
        {
            var appFile = MapToAppFileDto(src.AppFileEntry);

            var changeType = MapToAppFileChangeTypeDto(src.ChangeType);

            return new AppFileEntryChangeDto(appFile, changeType, src.Content);
        }
    }
}