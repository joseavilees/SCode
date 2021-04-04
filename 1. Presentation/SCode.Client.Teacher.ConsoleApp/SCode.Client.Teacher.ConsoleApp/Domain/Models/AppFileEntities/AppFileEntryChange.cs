namespace SCode.Client.Teacher.ConsoleApp.Domain.Models.AppFileEntities
{
    public class AppFileEntryChange
    {
        public AppFileEntry AppFileEntry { get; }
        public AppFileEntryChangeType ChangeType { get; }

        public string Content { get; }

        public AppFileEntryChange(AppFileEntry appFileEntry, AppFileEntryChangeType changeType, string content = null)
        {
            AppFileEntry = appFileEntry;
            ChangeType = changeType;
            Content = content;
        }
    }
}
