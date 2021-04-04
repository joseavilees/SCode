using MediatR;
using SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities;

namespace SCode.Client.Teacher.ConsoleApp.Domain.Events
{
    public class FileSystemEntryChangedEvent : INotification
    {
        public FileSystemEntryChange Change { get; }

        public FileSystemEntryChangedEvent(FileSystemEntryChange change)
        {
            Change = change;
        }
    }
}