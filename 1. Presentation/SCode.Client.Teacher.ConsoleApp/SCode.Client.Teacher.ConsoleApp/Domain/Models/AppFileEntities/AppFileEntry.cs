namespace SCode.Client.Teacher.ConsoleApp.Domain.Models.AppFileEntities
{
    public class AppFileEntry
    {
        public int Id { get; }
        public int ParentId { get; }

        public string Name { get; }
        public bool IsDirectory { get; }

        public AppFileEntry(int id, int parentId, string name, bool isDirectory)
        {
            Id = id;
            ParentId = parentId;
            
            Name = name;
            
            IsDirectory = isDirectory;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}