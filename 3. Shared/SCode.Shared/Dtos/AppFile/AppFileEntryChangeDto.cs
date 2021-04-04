namespace SCode.Shared.Dtos.AppFile
{
    public class AppFileEntryChangeDto
    {
        public AppFileEntryDto AppFileEntry { get; set; }

        public AppFileEntryChangeTypeDto ChangeType { get; set; }

        public string Content { get; set; }

        public AppFileEntryChangeDto(AppFileEntryDto appFileEntry, AppFileEntryChangeTypeDto changeType,
            string content = null)
        {
            AppFileEntry = appFileEntry;
            ChangeType = changeType;
            Content = content;
        }
    }
}