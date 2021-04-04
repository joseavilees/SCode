using System;

namespace SCode.Client.Teacher.ConsoleApp.Domain.Models.FileSystemEntryChangeEntities
{
    public class FileSystemEntryChange : IEquatable<FileSystemEntryChange>
    {
        public string Name { get; }
        public string Path { get; }

        /// <summary>
        /// Antigua ruta en si se trada de un renombre
        /// </summary>
        public string OldPath { get; }
        
        public FileSystemEntryChangeType ChangeType { get; }

        public FileSystemEntryChange(FileSystemEntryChangeType changeType, string name, string path,
            string oldPath = null)
        {
            Name = name;
            Path = path;
            OldPath = oldPath;
            ChangeType = changeType;

            if (changeType == FileSystemEntryChangeType.Renamed && oldPath == null)
            {
                throw new ArgumentNullException($"{nameof(oldPath)} es requerido en los eventos de renombre");
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as FileSystemEntryChange);
        }

        public bool Equals(FileSystemEntryChange other)
        {
            return other != null &&
                   Path == other.Path &&
                   OldPath == other.OldPath &&
                   ChangeType == other.ChangeType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Path, OldPath, ChangeType);
        }
    }
}
